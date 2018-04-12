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

        public IEnumerable<Database> Targets { get; set; }

        public string Query { get; set; }

        public int ConnectionTimeout { get; set; }

        public int CommandTimeout { get; set; }

        public bool Sequential { get; set; }

        public int Parallelism { get; set; }

        public bool ShowIpAddress { get; set; }

        public bool ShowServerName { get; set; }

        public bool ShowDatabaseName { get; set; }

        public bool StartKeyPress { get; set; }

        public bool StopKeyPress { get; set; }

        public bool ShowNulls { get; set; }

        public bool Progress { get; set; }

        public string NullsColor { get; set; }

        public bool ShowLogSheet { get; set; }

        public bool ShowParameterSheet { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Used by JsonConvert")]
        private class JsonTargets
        {
            public List<Database> DatabaseList { get; set; }
        }

        private static Parameters _instance;

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
                Query = File.ReadAllText(parsedResult.QueryFile);
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
                targets = File.ReadAllText(parsedResult.TargetsFile);
            }

            Targets = ParseTargets(targets);

            OutputFile = parsedResult.OutputFile;
            OutputDirectory = parsedResult.OutputDirectory ?? Directory.GetCurrentDirectory();
            Overwrite = parsedResult.Overwrite;
            ConnectionTimeout = parsedResult.ConnectionTimeout;
            CommandTimeout = parsedResult.CommandTimeout;
            Sequential = parsedResult.Sequential;
            Parallelism = parsedResult.Parallelism;
            ShowIpAddress = parsedResult.ShowIpAddress;
            ShowServerName = parsedResult.ShowServerName;
            ShowDatabaseName = parsedResult.ShowDatabaseName;
            StartKeyPress = parsedResult.StartKeyPress;
            StopKeyPress = parsedResult.StopKeyPress;
            ShowNulls = parsedResult.ShowNulls;
            Progress = parsedResult.Progress;
            NullsColor = parsedResult.NullsColor;

            ShowLogSheet = parsedResult.ShowLogSheet;
            ShowParameterSheet = parsedResult.ShowParameterSheet;

            ThrowIfInvalidParameter();
        }

        private static IEnumerable<Database> ParseTargets(string parsedResultTargets)
        {
            if (string.IsNullOrWhiteSpace(parsedResultTargets))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(parsedResultTargets));
            }

            var jsonTargets = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonTargets>(parsedResultTargets);

            var databaseArray = jsonTargets.DatabaseList.ToArray();
            Shuffler.ShuffleArray(databaseArray);

            return databaseArray;
        }

        public static string TargetsToJsonString(IEnumerable<Database> structuredTargets)
        {
            if (structuredTargets == null)
            {
                throw new ArgumentNullException(nameof(structuredTargets));
            }

            var targets = new JsonTargets {DatabaseList = structuredTargets.ToList()};
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

            if (!Targets.Any())
            {
                throw new ArgumentException("Targets cannot be empty.");
            }

            foreach (var database in Targets)
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

            uint nullsColorValue;

            if (!uint.TryParse(NullsColor, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out nullsColorValue))
            {
                throw new ArgumentException("NullsColor is not a valid hexadecimal value.");
            }

            if (nullsColorValue > 0xFFFFFF)
            {
                throw new ArgumentException("NullsColor is not a valid 24bits RGB color value.");
            }
        }

        public override string ToString()
        {
            return $"Query = \"{Query}\"";
        }
    }
}