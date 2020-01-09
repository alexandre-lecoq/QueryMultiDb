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
        /// <param name="executionResults">A collection of execution results.</param>
        /// <returns>The collection of tables.</returns>
        /// <remarks>
        /// If the results cannot be merged, an empty collection of tables should be returned and every issue should be logged.
        /// </remarks>
        public override ICollection<Table> MergeResults(ICollection<ExecutionResult> executionResults)
        {
            if (executionResults == null)
            {
                throw new ArgumentNullException(nameof(executionResults), "Parameter cannot be null.");
            }

            if (executionResults.Count == 0)
            {
                Logger.Warn("Execution did not yield any results.");
                Logger.Error("No data will be exported.");
                return new List<Table>(0);
            }
            
            if (!AllResultsCanBeMerged(executionResults))
            {
                Logger.Warn("Not all execution results can me merged. It is likely table sets aren't all identical.");
                Logger.Error("No data will be exported.");
                return new List<Table>(0);
            }

            WarnAboutMissingTableSets(executionResults);
            var resultTemplate = GetFirstResultWithNonEmptyTableSet(executionResults);

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

                MergeTables(executionResults, resultTemplate, TableSelector, tableSet);
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
            var allTablesFormatsAreIdentical = result.All(executionResult => CanBeMerged(executionResultTemplate, executionResult));

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
                    var columnCount = tableRow.ItemArray.Length;
                    minColumnCount = Math.Min(minColumnCount, columnCount);
                    maxColumnCount = Math.Max(maxColumnCount, columnCount);
                    var newRow = new TableRow(tableRow.ItemArray);
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
            var table = tableSelector(resultTemplate);

            if (!table.HasValue)
            {
                throw new Exception("Selected execution template table does not contain expected table");
            }

            return table.Value.Columns;
        }

        private static bool CanBeMerged(ExecutionResult left, ExecutionResult right)
        {
            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            if (left.TableSet == null && right.TableSet == null)
            {
                return true;
            }

            if (left.TableSet == null || right.TableSet == null)
            {
                LogTableComparisonWarning(left, right, "One of them does not contain any tables");
                return false;
            }
 
            if (left.TableSet.Count != right.TableSet.Count)
            {
                if (left.TableSet.Count == 0 || right.TableSet.Count == 0)
                {
                    LogTableComparisonWarning(left, right, $"Results can be merged although they have different number of tables because one of them has no table : {left.TableSet.Count} vs {right.TableSet.Count}");
                    return true;
                }

                LogTableComparisonWarning(left, right, $"Results have different number of tables : {left.TableSet.Count} vs {right.TableSet.Count}");
                return false;
            }

            for (var i = 0; i < left.TableSet.Count; i++)
            {
                var thisTable = left.TableSet[i];
                var otherTable = right.TableSet[i];

                var isIdentical = thisTable.HasIdenticalColumns(otherTable);

                if (!isIdentical)
                {
                    LogTableComparisonWarning(left, right, $"Tables at index #{i} have different column set");
                    return false;
                }
            }

            return true;
        }

        private static void LogTableComparisonWarning(ExecutionResult left, ExecutionResult right, string specificMessage)
        {
            var message = $"Tables are not identical. In {left.Database.ServerName} {left.Database.DatabaseName} and {right.Database.ServerName} {right.Database.DatabaseName}. {specificMessage}.";
            Logger.Warn(message);
        }
    }
}
