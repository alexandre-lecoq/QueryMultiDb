using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryMultiDb.DataMerger
{
    public class ConservativeDataMerger : DataMerger
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override string Name => "Conservative Data Merger";
        
        /// <summary>
        /// Merges execution results into a collection of tables.
        /// </summary>
        /// <param name="result">A collection of execution results.</param>
        /// <returns>The collection of tables.</returns>
        /// <remarks>
        /// If the results cannot be merged, an empty collection of tables should be returned and every issue should be logged.
        /// </remarks>
        public override ICollection<Table> MergeResults(ICollection<ExecutionResult> result)
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
            
            if (!AllResultsCanBeMerged(result))
            {
                Logger.Warn("Not all execution results can me merged. It is likely table sets aren't all identical.");
                Logger.Error("No data will be exported.");
                return new List<Table>(0);
            }

            WarnAboutMissingTableSets(result);
            var resultTemplate = GetFirstResultWithNonEmptyTableSet(result);

            if (resultTemplate == null)
            {
                Logger.Warn("Execution did not yield any table sets.");
                Logger.Error("No data will be exported.");
                return new List<Table>(0);
            }

            var tableCount = resultTemplate.TableSet.Count;
            var tableSet = new List<Table>(tableCount);

            for (var tableIndex = 0; tableIndex < tableCount; tableIndex++)
            {
                var index = tableIndex;

                Table? TableSelector(ExecutionResult r)
                {
                    if (r.TableSet.Count > index)
                    {
                        return r.TableSet[index];
                    }

                    return null;
                }

                MergeTables(result, resultTemplate, TableSelector, tableSet);
            }

            if (Parameters.Instance.ShowInformationMessages)
            {
                MergeTables(result, resultTemplate, r => r.InformationMessagesTable, tableSet);
            }

            return tableSet;
        }

        private static void MergeTables(ICollection<ExecutionResult> result, ExecutionResult resultTemplate, Func<ExecutionResult, Table?> tableSelector, List<Table> tableSet)
        {
            var columns = ComputeColumnSet(resultTemplate, tableSelector);
            var rows = ComputeRowSet(result, tableSelector);
            var tableId = ComputeTableId(resultTemplate, tableSelector);
            var destinationTable = new Table(columns, rows, tableId);

            if (destinationTable.Rows.Count > 0)
            {
                tableSet.Add(destinationTable);
            }
            else
            {
                Logger.Info($"Merged table '{destinationTable.Id}' was dropped because it was empty.");
            }
        }
        
        private static ExecutionResult GetFirstResultWithNonEmptyTableSet(ICollection<ExecutionResult> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (result.Count == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(result));
            }

            var executionResultTemplate = result.FirstOrDefault(r => r.TableSet.Count != 0);

            return executionResultTemplate;
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
                    $"Execution result for {executionResult.Database.DatabaseName} in {executionResult.Database.ServerName} contains a null table set.");
            }

            var emptyTableSetsResults = result.Where(executionResult => executionResult.TableSet.Count == 0).ToList();

            foreach (var executionResult in emptyTableSetsResults)
            {
                Logger.Warn(
                    $"Execution result for {executionResult.Database.DatabaseName} in {executionResult.Database.ServerName} contains an empty table set.");
            }
        }

        private static bool AllResultsCanBeMerged(ICollection<ExecutionResult> result)
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
            var allTablesFormatsAreIdentical = result.All(executionResult => executionResultTemplate.CanBeMerged(executionResult));

            return allTablesFormatsAreIdentical;
        }

        private static ICollection<TableRow> ComputeRowSet(ICollection<ExecutionResult> result, Func<ExecutionResult, Table?> tableSelector)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (tableSelector == null)
            {
                throw new ArgumentNullException(nameof(tableSelector));
            }

            var tableRows = new List<TableRow>();

            var minColumnCount = 0;
            var maxColumnCount = 0;

            foreach (var executionResult in result)
            {
                var table = tableSelector(executionResult);

                if (!table.HasValue)
                {
                    var message = $"Execution result {executionResult} does not contain expected table.";
                    Logger.Warn(message);
                    continue;
                }

                var sourceTable = table.Value;

                foreach (var tableRow in sourceTable.Rows)
                {
                    var builtInItems = new List<object>(10);

                    if (Parameters.Instance.ShowServerName)
                    {
                        builtInItems.Add(executionResult.Database.ServerName);
                    }

                    if (Parameters.Instance.ShowIpAddress)
                    {
                        var ip = DnsResolverWithCache.Instance.Resolve(executionResult.Database.ServerName);
                        builtInItems.Add(ip?.ToString() ?? string.Empty);
                    }

                    if (Parameters.Instance.ShowDatabaseName)
                    {
                        builtInItems.Add(executionResult.Database.DatabaseName);
                    }

                    if (Parameters.Instance.ShowExtraColumns)
                    {
                        if (!Parameters.Instance.Targets.EmptyExtraValues[0])
                        {
                            builtInItems.Add(executionResult.Database.ExtraValue1);
                        }

                        if (!Parameters.Instance.Targets.EmptyExtraValues[1])
                        {
                            builtInItems.Add(executionResult.Database.ExtraValue2);
                        }

                        if (!Parameters.Instance.Targets.EmptyExtraValues[2])
                        {
                            builtInItems.Add(executionResult.Database.ExtraValue3);
                        }

                        if (!Parameters.Instance.Targets.EmptyExtraValues[3])
                        {
                            builtInItems.Add(executionResult.Database.ExtraValue4);
                        }

                        if (!Parameters.Instance.Targets.EmptyExtraValues[4])
                        {
                            builtInItems.Add(executionResult.Database.ExtraValue5);
                        }

                        if (!Parameters.Instance.Targets.EmptyExtraValues[5])
                        {
                            builtInItems.Add(executionResult.Database.ExtraValue6);
                        }
                    }

                    var columnCount = tableRow.ItemArray.Length;
                    minColumnCount = Math.Min(minColumnCount, columnCount);
                    maxColumnCount = Math.Max(maxColumnCount, columnCount);
                    var items = new object[builtInItems.Count + columnCount];
                    builtInItems.CopyTo(items, 0);
                    tableRow.ItemArray.CopyTo(items, builtInItems.Count);
                    var newRow = new TableRow(items);
                    tableRows.Add(newRow);
                }
            }

            if (minColumnCount != 0 && minColumnCount != maxColumnCount)
            {
                var message = $"Unexpected inconsistent number of columns in row set. Found {minColumnCount} to {maxColumnCount} columns in table's rows. Tables cannot be merged.";
                throw new Exception(message);
            }

            return tableRows;
        }

        private static string ComputeTableId(ExecutionResult resultTemplate, Func<ExecutionResult, Table?> tableSelector)
        {
            var table = tableSelector(resultTemplate);
            
            if (!table.HasValue)
            {
                throw new Exception("Selected execution template table does not contain expected table");
            }

            var tableId = table.Value.Id.StartsWith("__", StringComparison.InvariantCulture) ? table.Value.Id : null;

            return tableId;
        }

        private static TableColumn[] ComputeColumnSet(ExecutionResult resultTemplate, Func<ExecutionResult, Table?> tableSelector)
        {
            var builtInColumnSet = new List<TableColumn>(10);

            if (Parameters.Instance.ShowServerName)
            {
                builtInColumnSet.Add(new TableColumn("_ServerName", typeof(string)));
            }

            if (Parameters.Instance.ShowIpAddress)
            {
                builtInColumnSet.Add(new TableColumn("_ServerIp", typeof(string)));
            }

            if (Parameters.Instance.ShowDatabaseName)
            {
                builtInColumnSet.Add(new TableColumn("_DatabaseName", typeof(string)));
            }

            if (Parameters.Instance.ShowExtraColumns)
            {
                var titlesSettings = Parameters.Instance.Targets.ExtraValueTitles;

                for (var i = 0; i < 6; i++)
                {
                    if (!Parameters.Instance.Targets.EmptyExtraValues[i])
                    {
                        builtInColumnSet.Add(new TableColumn(titlesSettings[i], typeof(string)));
                    }
                }
            }

            var table = tableSelector(resultTemplate);

            if (!table.HasValue)
            {
                throw new Exception("Selected execution template table does not contain expected table");
            }

            var computedColumns = table.Value.Columns;
            var destinationColumnSet = new TableColumn[builtInColumnSet.Count + computedColumns.Length];
            builtInColumnSet.CopyTo(destinationColumnSet, 0);
            computedColumns.CopyTo(destinationColumnSet, builtInColumnSet.Count);
            return destinationColumnSet;
        }
    }
}
