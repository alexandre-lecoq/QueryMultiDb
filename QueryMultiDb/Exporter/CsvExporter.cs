using CsvHelper;
using CsvHelper.Configuration;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace QueryMultiDb.Exporter
{
    public class CsvExporter : Exporter
    {
        private const int MaximumFileNameLength = 255;
        private const string CsvFileExtension = ".csv";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override string Name => "CSV";

        public override void Generate(ICollection<Table> inputTables, Stream outputStream)
        {
            Logger.Info("Created ZIP file.");

            var progressReporter = new ProgressReporter("CsvExporter", Parameters.Instance.Targets.Databases.Count(), s => Console.Error.WriteLine(s));

            using (var zipArchive = new ZipArchive(outputStream, ZipArchiveMode.Create))
            {
                var tableIndex = 0;

                foreach (var table in inputTables)
                {
                    Logger.Info("Adding new CSV file.");

                    var partName = GetPartName(table, tableIndex);
                    AddCsv(zipArchive, table, partName);
                    progressReporter.Increment();
                    tableIndex++;
                }

                progressReporter.Done();

                MemoryManager.Clean();

                var target = GetLogTableTarget();

                Logger.Info("CSV file logging horizon. Check console output to see beyond horizon.");

                if (Parameters.Instance.ShowLogSheet)
                {
                    var logTable = target.Logs;
                    var partName = GetPartName(logTable, tableIndex++);
                    AddCsv(zipArchive, logTable, partName);
                }

                if (Parameters.Instance.ShowParameterSheet)
                {
                    var parameterTable = ParametersToTable(Parameters.Instance);
                    var partName = GetPartName(parameterTable, tableIndex++);
                    AddCsv(zipArchive, parameterTable, partName);
                }
                
                MemoryManager.Clean();

                Logger.Info("Finalizing CSV file writing.");
            }
        }

        private static void AddCsv(ZipArchive zipArchive, Table table, string partName = null)
        {
            if (zipArchive == null)
            {
                throw new ArgumentNullException(nameof(zipArchive));
            }

            var realPartName = partName ?? "Part." + table.Id;

            string truncatedPartName;

            if (realPartName.Length > MaximumFileNameLength)
            {
                truncatedPartName = realPartName.Substring(0, MaximumFileNameLength);
                Logger.Warn($"Part name was truncated. Full name was '{realPartName}', truncated name is '{truncatedPartName}'.");
            }
            else
            {
                truncatedPartName = realPartName;
            }

            var archiveEntry = zipArchive.CreateEntry(truncatedPartName + CsvFileExtension);
            var stream = archiveEntry.Open();

            var configuration = new Configuration
            {
                Encoding = Encoding.UTF8,
                Delimiter = Parameters.Instance.CsvDelimiter
            };

            var binaryBuffers = new List<KeyValuePair<string, byte[]>>();

            using (var streamWriter = new StreamWriter(stream, Encoding.UTF8))
            using (var csvWriter = new CsvWriter(streamWriter, configuration))
            {
                var columnSet = GenerateValidAndUniqueColumnNames(table.Columns);

                foreach (var column in columnSet)
                {
                    csvWriter.WriteField(column.ColumnName);
                }

                csvWriter.NextRecord();

                foreach (var tableRow in table.Rows)
                {
                    for (var columnIndex = 0; columnIndex < columnSet.Length; columnIndex++)
                    {
                        var data = tableRow.ItemArray[columnIndex];
                        var text = GetCsvString(data);
                        var referencePath = ExtractReferencePath(text);

                        if (referencePath != null)
                        {
                            var bufferWithPath = new KeyValuePair<string, byte[]>(referencePath, (byte[])data);
                            binaryBuffers.Add(bufferWithPath);
                        }

                        csvWriter.WriteField(text);
                    }

                    csvWriter.NextRecord();
                }
            }

            foreach (var kvp in binaryBuffers)
            {
                var binaryArchiveEntry = zipArchive.CreateEntry(kvp.Key);
                var binaryStream = binaryArchiveEntry.Open();
                binaryStream.Write(kvp.Value, 0, kvp.Value.Length);
                binaryStream.Flush();
                binaryStream.Dispose();
            }
        }

        private static string GetCsvString(object item)
        {
            switch (item)
            {
                case null:
                    throw new ArgumentNullException(nameof(item), "Parameter cannot be null.");
                case DateTime dateTime:
                    return GetCsvStringAsDateTime(dateTime);
                case DBNull _:
                    return GetCsvStringAsNull();
                case byte[] bytes:
                    return ByteArrayToString(bytes);
                default:
                    return GetCsvStringAsDefault(item);
            }
        }

        private static string GetCsvStringAsDateTime(DateTime dateTime)
        {
            return dateTime.ToString("O");
        }

        private static string GetCsvStringAsNull()
        {
            var text = Parameters.Instance.ShowNulls ? "NULL" : string.Empty;

            return text;
        }

        private static string GetCsvStringAsDefault(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return item.ToString();
        }
    }
}
