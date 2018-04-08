using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace QueryMultiDb
{
    public static class DataMerger
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Merges execution results into a collection of tables.
        /// </summary>
        /// <param name="result">A collection of execetion results.</param>
        /// <returns>The collection of tables.</returns>
        /// <remarks>
        /// If the results cannot be merged, an empty collection of tables should be returned and every issue should be logged.
        /// </remarks>
        public static ICollection<Table> MergeResults(ICollection<ExecutionResult> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result), "Parameter cannot be null.");
            }

            if (result.Count == 0)
            {
                Logger.Warn("Execution did not yield any results.");
                Logger.Error("No data will be exported.");
                return new List<Table>(0);
            }
            
            if (!AllTablesFormatsAreIdentical(result))
            {
                Logger.Warn("Not all execution results yielded identical tables.");
                return new List<Table>(0);
            }

            WarnAboutMissingTableSets(result);
            var tableCount = GetFirstResultTableCount(result);
            var tableSet = new List<Table>(tableCount);

            for (var tableIndex = 0; tableIndex < tableCount; tableIndex++)
            {
                var builtInColumnSet = new List<TableColumn>(3);
                builtInColumnSet.Add(new TableColumn("_ServerName", typeof(string)));

                if (Parameters.Instance.IncludeIP)
                {
                    builtInColumnSet.Add(new TableColumn("_ServerIp", typeof(string)));
                }

                builtInColumnSet.Add(new TableColumn("_DatabaseName", typeof(string)));

                var table = result.First().TableSet[tableIndex];
                var computedColumns = table.Columns;
                var destinationColumnSet = new TableColumn[builtInColumnSet.Count + computedColumns.Length];
                builtInColumnSet.CopyTo(destinationColumnSet, 0);
                computedColumns.CopyTo(destinationColumnSet, builtInColumnSet.Count);
                var rows = ComputeRowSet(result, tableIndex);
                var tableId = table.Id.StartsWith("__", StringComparison.InvariantCulture) ? table.Id : null;
                var destinationTable = new Table(destinationColumnSet, rows, tableId);

                if (destinationTable.Rows.Count > 0)
                {
                    tableSet.Add(destinationTable);
                }
                else
                {
                    Logger.Info($"Merged table '{destinationTable.Id}' (Index : {tableIndex}) was dropped because it was empty.");
                }
            }

            return tableSet;
        }

        private static int GetFirstResultTableCount(ICollection<ExecutionResult> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (result.Count == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(result));
            }

            var executionResultTemplate = result.First();
            var tableCount = executionResultTemplate.TableSet.Count;

            return tableCount;
        }

        private static void WarnAboutMissingTableSets(ICollection<ExecutionResult> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (result.Count == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(result));
            }

            var nullTableSetsResults = result.Where(executionResult => executionResult.TableSet == null).ToList();

            foreach (var executionResult in nullTableSetsResults)
            {
                Logger.Warn(
                    $"Execution result for {executionResult.DatabaseName} in {executionResult.ServerName} contains a null table set.");
            }

            var emptyTableSetsResults = result.Where(executionResult => executionResult.TableSet.Count == 0).ToList();

            foreach (var executionResult in emptyTableSetsResults)
            {
                Logger.Warn(
                    $"Execution result for {executionResult.DatabaseName} in {executionResult.ServerName} contains an empty table set.");
            }
        }

        private static bool AllTablesFormatsAreIdentical(ICollection<ExecutionResult> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (result.Count == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(result));
            }

            var executionResultTemplate = result.First();
            var allTablesFormatsAreIdentical = result.All(executionResult => executionResultTemplate.HasIdenticalTableAndColumns(executionResult));

            return allTablesFormatsAreIdentical;
        }

        private static ICollection<TableRow> ComputeRowSet(ICollection<ExecutionResult> result, int tableIndex)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (tableIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tableIndex));
            }

            var tableRows = new List<TableRow>();

            foreach (var executionResult in result)
            {
                var sourceTable = executionResult.TableSet[tableIndex];

                foreach (var tableRow in sourceTable.Rows)
                {
                    var builtInItems = new List<object>(9);
                    builtInItems.Add(executionResult.ServerName);

                    if (Parameters.Instance.IncludeIP)
                    {
                        var ip = DnsResolverWithCache.Instance.Resolve(executionResult.ServerName);
                        builtInItems.Add(ip?.ToString() ?? string.Empty);
                    }

                    builtInItems.Add(executionResult.DatabaseName);

                    var items = new object[builtInItems.Count + tableRow.ItemArray.Length];
                    builtInItems.CopyTo(items, 0);
                    tableRow.ItemArray.CopyTo(items, builtInItems.Count);
                    var newRow = new TableRow(items);
                    tableRows.Add(newRow);
                }
            }

            return tableRows;
        }
    }
}
