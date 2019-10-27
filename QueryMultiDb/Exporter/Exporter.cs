using NLog;
using NLog.Targets.Wrappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace QueryMultiDb.Exporter
{
    public abstract class Exporter : IExporter
    {
        private const string BinaryDataString = "data:";
        private const string BinaryDataNoneString = BinaryDataString + "none";
        private const string BinaryDataBase10String = BinaryDataString + "base10,";
        private const string BinaryDataBase16String = BinaryDataString + "base16,";
        private const string BinaryDataBase64String = BinaryDataString + "base64,";
        private const string BinaryDataReferenceString = BinaryDataString + "reference,";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public abstract string Name { get; }

        public void Generate(ICollection<Table> inputTables)
        {
            if (inputTables == null)
            {
                throw new ArgumentNullException(nameof(inputTables), "Parameter cannot be null.");
            }

            var destination = Path.IsPathRooted(Parameters.Instance.OutputFile)
                ? Parameters.Instance.OutputFile
                : Path.Combine(Parameters.Instance.OutputDirectory, Parameters.Instance.OutputFile);

            Logger.Info($"Creating {Name} file '{destination}'");
            MemoryManager.Clean();

            using (var fileStream = new FileStream(destination, FileMode.CreateNew))
            {
                Generate(inputTables, fileStream);
            }

            Logger.Info($"Output {Name} closed after generation.");
            MemoryManager.Clean();
        }

        public abstract void Generate(ICollection<Table> inputTables, Stream outputStream);

        /// <summary>
        /// Generates valid and unique column names.
        /// </summary>
        /// <param name="tableColumns">An arbitrary set of columns.</param>
        /// <returns>A set of columns with unique and valid names.</returns>
        /// <remarks>
        /// The returned column set has names with the following properties:
        /// * All columns have names
        /// * All names are unique
        /// * All names are trimmed
        /// This matches the requirement for excel column names, but might help with any output format.
        /// </remarks>
        protected static TableColumn[] GenerateValidAndUniqueColumnNames(TableColumn[] tableColumns)
        {
            if (tableColumns == null)
            {
                throw new ArgumentNullException(nameof(tableColumns));
            }

            var columnNames = new string[tableColumns.Length];

            for (var i = 0; i < tableColumns.Length; i++)
            {
                var columnName = string.IsNullOrEmpty(tableColumns[i].ColumnName)
                    ? "Column"
                    : tableColumns[i].ColumnName;
                columnNames[i] = columnName;
            }
            
            var nameCounts = new Dictionary<string, int>();

            for (var i = 0; i < tableColumns.Length; i++)
            {
                var columnName = columnNames[i];

                if (nameCounts.ContainsKey(columnName))
                {
                    nameCounts[columnName]++;
                }
                else
                {
                    nameCounts.Add(columnName, 1);
                }

                if (nameCounts[columnName] > 1)
                {
                    columnNames[i] += nameCounts[columnName];
                }
            }

            var outputColumnSet = new TableColumn[tableColumns.Length];

            for (var i = 0; i < tableColumns.Length; i++)
            {
                var trimmedColumnName = columnNames[i].Trim();

                if (trimmedColumnName.Length != columnNames[i].Length)
                {
                    Logger.Warn($"Column name '{columnNames[i]}' contains white spaces. Name was trimmed to '{trimmedColumnName}'.");
                }

                outputColumnSet[i] = new TableColumn(trimmedColumnName, tableColumns[i].DataType);
            }

            return outputColumnSet;
        }

        protected static string GetPartName(Table table, int tableIndex)
        {
            var sheetNameById = GetPartNameFromTableId(table.Id);
            var sheetNameByParameter = GetPartNameFromParameter(tableIndex);
            var sheetName = sheetNameById ?? sheetNameByParameter;

            return sheetName;
        }

        private static string GetPartNameFromTableId(string tableId)
        {
            if (string.IsNullOrWhiteSpace(tableId))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(tableId));
            }

            switch (tableId)
            {
                case Table.InformationMessagesId:
                    return "Information messages";

                case Table.LogsId:
                    return "Logs";

                case Table.CommandLineParametersId:
                    return "Parameters";

                default:
                    return null;
            }
        }

        private static string GetPartNameFromParameter(int tableIndex)
        {
            if (tableIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tableIndex));
            }

            var labels = Parameters.Instance.SheetLabels;

            if (tableIndex >= labels.Count)
            {
                return null;
            }

            var label = labels.ElementAt(tableIndex);

            return label;
        }

        protected static Table ParametersToTable(Parameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var parameterColumns = new TableColumn[2];
            parameterColumns[0] = new TableColumn("Parameter", typeof(string));
            parameterColumns[1] = new TableColumn("Value", typeof(string));

            var parameterRows = new List<TableRow>
            {
                CreateParameterRow("OutputDirectory", parameters.OutputDirectory),
                CreateParameterRow("OutputFile", parameters.OutputFile),
                CreateParameterRow("Overwrite", parameters.Overwrite),
                CreateParameterRow("Targets", Parameters.TargetsToJsonString(parameters.Targets)),
                CreateParameterRow("Query", parameters.Query),
                CreateParameterRow("ConnectionTimeout", parameters.ConnectionTimeout),
                CreateParameterRow("CommandTimeout", parameters.CommandTimeout),
                CreateParameterRow("Sequential", parameters.Sequential),
                CreateParameterRow("Parallelism", parameters.Parallelism),
                CreateParameterRow("ShowIpAddress", parameters.ShowIpAddress),
                CreateParameterRow("StartKeyPress", parameters.StartKeyPress),
                CreateParameterRow("StopKeyPress", parameters.StopKeyPress),
                CreateParameterRow("ShowNulls", parameters.ShowNulls),
                CreateParameterRow("Progress", parameters.Progress),
                CreateParameterRow("NullsColor", parameters.NullsColor),
                CreateParameterRow("ShowLogSheet", parameters.ShowLogSheet),
                CreateParameterRow("ShowParameterSheet", parameters.ShowParameterSheet),
                CreateParameterRow("ShowServerName", parameters.ShowServerName),
                CreateParameterRow("ShowDatabaseName", parameters.ShowDatabaseName),
                CreateParameterRow("ShowExtraColumns", parameters.ShowExtraColumns),
                CreateParameterRow("ShowInformationMessages", parameters.ShowInformationMessages),
                CreateParameterRow("SheetLabels", string.Join(", ", parameters.SheetLabels)),
                CreateParameterRow("DiscardResults", parameters.DiscardResults),
                CreateParameterRow("ApplicationName", parameters.ApplicationName),
                CreateParameterRow("Exporter", parameters.Exporter),
                CreateParameterRow("CsvDelimiter", parameters.CsvDelimiter)
            };

            var parameterTable = new Table(parameterColumns, parameterRows, Table.CommandLineParametersId);

            return parameterTable;
        }

        private static TableRow CreateParameterRow(string parameter, object value)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(parameter));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var items = new object[2];
            items[0] = parameter;
            items[1] = value;
            var tableRow = new TableRow(items);

            return tableRow;
        }

        protected static TableTarget GetLogTableTarget()
        {
            var flushedTableTarget = LogManager.Configuration.FindTargetByName<AutoFlushTargetWrapper>("flushedTableTarget");
            var target = flushedTableTarget.WrappedTarget as TableTarget;

            if (target == null)
            {
                throw new InvalidOperationException(
                    "Logger's wrapped table target could not be recovered. It should never happens as this target should be added very early in Program.Main().");
            }

            return target;
        }

        protected static string ByteArrayToString(byte[] item, bool useExternalReferences = true)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            
            if (item.Length == 0)
            {
                return BinaryDataNoneString;
            }

            if (item.Length <= 4)
            {
                var base10String = ToDecimalString(item);

                return BinaryDataBase10String + base10String;
            }

            if (item.Length <= 64)
            {
                var base16String = ToHexString(item);

                return BinaryDataBase16String + base16String;
            }

            if (item.Length <= 262144 || !useExternalReferences)
            {
                var base64String = Convert.ToBase64String(item, Base64FormattingOptions.None);

                return BinaryDataBase64String + base64String;
            }
            
            var guidString = Guid.NewGuid().ToString("N");
            var pathString = string.Empty + // Empty string ensures character is not implicitly cast to integer.
                             guidString[0] + guidString[1] + "/" +
                             guidString[2] + guidString[3] + "/" +
                             guidString[4] + guidString[5] + "/" +
                             guidString[6] + guidString[7] + "/" +
                             guidString + "-" + item[0].ToString("x2");
            var referenceString = BinaryDataReferenceString + pathString;

            return referenceString;
        }

        protected static string ExtractReferencePath(string text)
        {
            if (!text.StartsWith(BinaryDataReferenceString))
                return null;

            var referencePath = text.Substring(BinaryDataReferenceString.Length, text.Length - BinaryDataReferenceString.Length);

            return referencePath;
        }

        private static string ToHexString(byte[] value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (value.Length == 0)
                return string.Empty;

            var outputLength = value.Length * 2;
            var charArray = new char[outputLength];
            var inputIndex = 0;

            for (var outputIndex = 0; outputIndex < outputLength; outputIndex += 2)
            {
                var b = value[inputIndex++];
                charArray[outputIndex] = GetHexValue(b / 16);
                charArray[outputIndex + 1] = GetHexValue(b % 16);
            }

            var str = new string(charArray, 0, charArray.Length);

            return str;
        }

        private static char GetHexValue(int i)
        {
            if (i < 10)
                return (char)(i + 48);

            return (char)(i - 10 + 65);
        }

        private static string ToDecimalString(byte[] value)
        {
            var integer = FromBigEndianByteArray(value);
            var str = integer.ToString();

            return str;
        }

        /// <summary>
        /// Creates a BigInteger from a big-endian byte array.
        /// </summary>
        /// <param name="value">The byte array.</param>
        /// <returns>The BigInteger.</returns>
        private static BigInteger FromBigEndianByteArray(byte[] value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "Value cannot be null.");
            }

            var temp = new byte[value.Length + 1];
            Array.Copy(value, 0, temp, 1, value.Length);
            Array.Reverse(temp);

            // The BigInteger will always be unsigned as the array will always end with 0x00.

            return new BigInteger(temp);
        }
    }
}
