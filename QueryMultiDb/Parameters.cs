using QueryMultiDb.Exporter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace QueryMultiDb
{
    public class Parameters
    {
        public string OutputDirectory { get; set; }

        public string OutputFile { get; set; }

        public bool Overwrite { get; set; }

        public TargetSet Targets { get; set; }

        public string Query { get; set; }

        public int ConnectionTimeout { get; set; }

        public int CommandTimeout { get; set; }

        public bool Sequential { get; set; }

        public int Parallelism { get; set; }

        public bool ShowIpAddress { get; set; }

        public bool ShowServerName { get; set; }

        public bool ShowDatabaseName { get; set; }

        public bool ShowExtraColumns { get; set; }
        
        public bool StartKeyPress { get; set; }

        public bool StopKeyPress { get; set; }

        public bool ShowNulls { get; set; }

        public bool Progress { get; set; }

        public string NullsColor { get; set; }

        public bool ShowLogSheet { get; set; }

        public bool ShowParameterSheet { get; set; }

        public bool ShowInformationMessages { get; set; }

        public ICollection<string> SheetLabels { get; set; }

        public bool DiscardResults { get; set; }

        public string ApplicationName { get; set; }

        public ExporterType Exporter { get; set; }

        public string CsvDelimiter { get; set; }

        public int Base10Threshold { get; set; }

        public int Base16Threshold { get; set; }

        public int Base64Threshold { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Used by JsonConvert")]
        private class JsonTargets
        {
            public List<Database> DatabaseList { get; set; }

            public string ExtraValue1Title { get; set; }

            public string ExtraValue2Title { get; set; }

            public string ExtraValue3Title { get; set; }

            public string ExtraValue4Title { get; set; }

            public string ExtraValue5Title { get; set; }

            public string ExtraValue6Title { get; set; }
        }

        private static Parameters _instance;

        public static bool IsInitialized => _instance != null;

        public static Parameters Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("Instance is not set.");
                }

                return _instance;
            }

            set
            {
                if (_instance != null)
                {
                    throw new InvalidOperationException("Instance is already set.");
                }

                _instance = value;
            }
        }

        public Parameters(CommandLineParameters parsedResult)
        {
            if (parsedResult == null)
            {
                throw new ArgumentNullException(nameof(parsedResult), "Parameter cannot be null.");
            }

            if (parsedResult.Query == null && parsedResult.QueryFile == null)
            {
                throw new ArgumentException("No target specified.");
            }

            var queryParameters = 0;

            if (parsedResult.Query != null)
            {
                queryParameters++;
            }

            if (parsedResult.QueryFile != null)
            {
                queryParameters++;
            }

            if (queryParameters > 1)
            {
                throw new ArgumentException("Cannot use more than one of constant or targets file for specifying query.");
            }

            Query = parsedResult.Query;

            if (parsedResult.QueryFile != null)
            {
                Query = File.ReadAllText(parsedResult.QueryFile.FullName);
            }

            if (parsedResult.Targets == null && parsedResult.TargetsStandardInput == false && parsedResult.TargetsFile == null)
            {
                throw new ArgumentException("No target specified.");
            }

            var targetParameters = 0;

            if (parsedResult.Targets != null)
            {
                targetParameters++;
            }

            if (parsedResult.TargetsStandardInput)
            {
                targetParameters++;
            }

            if (parsedResult.TargetsFile != null)
            {
                targetParameters++;
            }

            if (targetParameters > 1)
            {
                throw new ArgumentException("Cannot use more than one of standard input or constant or targets file for specifying targets.");
            }

            var targets = parsedResult.Targets;

            if (parsedResult.TargetsStandardInput)
            {
                targets = Console.In.ReadToEnd();
            }
            else if (parsedResult.TargetsFile != null)
            {
                targets = File.ReadAllText(parsedResult.TargetsFile.FullName);
            }

            Targets = ParseTargets(targets);

            OutputFile = parsedResult.OutputFile;
            OutputDirectory = parsedResult.OutputDirectory?.FullName ?? Directory.GetCurrentDirectory();
            Overwrite = parsedResult.Overwrite;
            ConnectionTimeout = parsedResult.ConnectionTimeout;
            CommandTimeout = parsedResult.CommandTimeout;
            Sequential = parsedResult.Sequential;
            Parallelism = parsedResult.Parallelism;
            ShowIpAddress = parsedResult.ShowIpAddress;
            ShowServerName = parsedResult.ShowServerName;
            ShowDatabaseName = parsedResult.ShowDatabaseName;
            ShowExtraColumns = parsedResult.ShowExtraColumns;
            StartKeyPress = parsedResult.StartKeyPress;
            StopKeyPress = parsedResult.StopKeyPress;
            ShowNulls = parsedResult.ShowNulls;
            Progress = parsedResult.Progress;
            NullsColor = parsedResult.NullsColor;
            ShowLogSheet = parsedResult.ShowLogSheet;
            ShowParameterSheet = parsedResult.ShowParameterSheet;
            ShowInformationMessages = parsedResult.ShowInformationMessages;
            SheetLabels = ParseSheetLabels(parsedResult.SheetLabels);
            DiscardResults = parsedResult.DiscardResults;
            ApplicationName = parsedResult.ApplicationName ?? string.Empty;
            Exporter = parsedResult.Exporter;
            CsvDelimiter = parsedResult.CsvDelimiter;
            Base10Threshold = parsedResult.Base10Threshold;
            Base16Threshold = parsedResult.Base16Threshold;
            Base64Threshold = parsedResult.Base64Threshold;

            ThrowIfInvalidParameter();
        }

        private static ICollection<string> ParseSheetLabels(string parsedResultSheetLabels)
        {
            if (parsedResultSheetLabels == null)
            {
                return new List<string>();
            }

            if (parsedResultSheetLabels.Trim() == string.Empty)
            {
                throw new ArgumentException("Value cannot be empty or whitespace.", nameof(parsedResultSheetLabels));
            }

            var splitLabels = parsedResultSheetLabels.Split(',');
            var trimmedLabels = splitLabels.Select(p => p.Trim());

            return trimmedLabels.ToList();
        }

        private static TargetSet ParseTargets(string parsedResultTargets)
        {
            if (string.IsNullOrWhiteSpace(parsedResultTargets))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(parsedResultTargets));
            }

            var jsonTargets = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonTargets>(parsedResultTargets);

            var databaseArray = jsonTargets.DatabaseList.ToArray();

            // This is very important to shuffle this array because its processing will likely be paralleled.
            // It drastically reduces the probability two problematic databases are close to each other in the array in the average case.
            // Doing so, it reduces the chance of having several threads blocked at the same time.
            // You can see this shuffling as a performance optimization.
            Shuffler.ShuffleArray(databaseArray);

            var extraValueTitles = new[]
            {
                jsonTargets.ExtraValue1Title,
                jsonTargets.ExtraValue2Title,
                jsonTargets.ExtraValue3Title,
                jsonTargets.ExtraValue4Title,
                jsonTargets.ExtraValue5Title,
                jsonTargets.ExtraValue6Title
            };

            for (var i = 0; i < extraValueTitles.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(extraValueTitles[i]))
                {
                    extraValueTitles[i] = $"ExtraValue{(i + 1)}";
                }
                else
                {
                    extraValueTitles[i] = extraValueTitles[i].Trim();
                }

                extraValueTitles[i] = "_" + extraValueTitles[i];
            }

            return new TargetSet(databaseArray, extraValueTitles);
        }

        public static string TargetsToJsonString(TargetSet structuredTargets)
        {
            if (structuredTargets == null)
            {
                throw new ArgumentNullException(nameof(structuredTargets));
            }

            var targets = new JsonTargets
            {
                DatabaseList = structuredTargets.Databases.ToList(),
                ExtraValue1Title = structuredTargets.ExtraValueTitles[0],
                ExtraValue2Title = structuredTargets.ExtraValueTitles[1],
                ExtraValue3Title = structuredTargets.ExtraValueTitles[2],
                ExtraValue4Title = structuredTargets.ExtraValueTitles[3],
                ExtraValue5Title = structuredTargets.ExtraValueTitles[4],
                ExtraValue6Title = structuredTargets.ExtraValueTitles[5]
            };
            var serializeTargets = Newtonsoft.Json.JsonConvert.SerializeObject(targets);

            return serializeTargets;
        }

        private void ThrowIfInvalidParameter()
        {
            if (string.IsNullOrWhiteSpace(Query))
            {
                throw new ArgumentException("Query cannot be null, empty or blank.");
            }

            if (Targets == null)
            {
                throw new ArgumentException("Targets cannot be null.");
            }

            if (!Targets.Databases.Any())
            {
                throw new ArgumentException("Targets' databases cannot be empty.");
            }

            foreach (var database in Targets.Databases)
            {
                if (string.IsNullOrWhiteSpace(database.ServerName))
                {
                    throw new ArgumentException("ServerName cannot be null, empty or blank.");
                }

                if (string.IsNullOrWhiteSpace(database.DatabaseName))
                {
                    throw new ArgumentException("DatabaseName cannot be null, empty or blank.");
                }
            }

            if (Targets.ExtraValueTitles == null)
            {
                throw new ArgumentException("Targets' ExtraValueTitles cannot be null.");
            }

            if (!Directory.Exists(OutputDirectory))
            {
                throw new ArgumentException("OutputDirectory does not exist.");
            }

            if (!Overwrite && File.Exists(OutputDirectory + "\\" + OutputFile))
            {

                throw new ArgumentException("OutputFile already exists in directory.");
            }

            if (ConnectionTimeout < 0)
            {
                throw new ArgumentException("Connection timeout cannot be negative.");
            }

            if (ConnectionTimeout == 0)
            {
                throw new ArgumentException("Connection timeout cannot be 0 (infinite).");
            }

            if (ConnectionTimeout > 604800)
            {
                throw new ArgumentException("Connection timeout cannot be more than 604800 seconds (one week).");
            }

            if (CommandTimeout < 0)
            {
                throw new ArgumentException("Command timeout cannot be negative.");
            }

            if (CommandTimeout > 604800)
            {
                throw new ArgumentException("Command timeout cannot be more than 604800 seconds (one week).");
            }

            if (Parallelism < 1)
            {
                throw new ArgumentException("Parallelism cannot be less than 1.");
            }

            if (Parallelism > 32)
            {
                throw new ArgumentException("Parallelism cannot be more than 32.");
            }

            if (!uint.TryParse(NullsColor, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var nullsColorValue))
            {
                throw new ArgumentException("NullsColor is not a valid hexadecimal value.");
            }

            if (nullsColorValue > 0xFFFFFF)
            {
                throw new ArgumentException("NullsColor is not a valid 24bits RGB color value.");
            }

            if (SheetLabels == null)
            {
                throw new ArgumentException("SheetLabels cannot be null.");
            }

            foreach (var label in SheetLabels)
            {
                if (string.IsNullOrWhiteSpace(label))
                {
                    throw new ArgumentException("Sheet label cannot be null, empty or blank.");
                }
            }

            if (!string.IsNullOrEmpty(ApplicationName) && ApplicationName.Trim() == string.Empty)
            {
                throw new ArgumentException("ApplicationName cannot be white spaces.");
            }

            if (Exporter != ExporterType.Excel && Exporter != ExporterType.Csv)
            {
                throw new ArgumentException("Exporter must be either \"csv\" or \"excel\".");
            }

            if (string.IsNullOrEmpty(CsvDelimiter))
            {
                throw new ArgumentException("CSV delimiter cannot be empty.");
            }

            if (CsvDelimiter.Length != 1)
            {
                throw new ArgumentException("CSV delimiter must be only one character.");
            }

            if (Base10Threshold < 0)
            {
                throw new ArgumentException("Base 10 threshold cannot be negative.");
            }

            if (Base16Threshold < 0)
            {
                throw new ArgumentException("Base 16 threshold cannot be negative.");
            }

            if (Base64Threshold < 0)
            {
                throw new ArgumentException("Base 64 threshold cannot be negative.");
            }
        }

        public override string ToString()
        {
            return $"Query = \"{Query}\"";
        }
    }
}