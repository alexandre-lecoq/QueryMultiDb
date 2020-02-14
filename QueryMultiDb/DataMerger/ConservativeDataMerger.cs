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

            WarnAboutMissingTableSets(executionResults);

            var buckets = MapToBuckets(executionResults);

            if (buckets == null)
            {
                Logger.Warn("Not all execution results can me merged. It is likely table sets aren't all identical.");
                Logger.Error("No data will be exported.");
                return new List<Table>(0);
            }

            var maxTablesInBuckets = buckets.Select(b => b.Count).Max();

            if (maxTablesInBuckets == 0)
            {
                Logger.Warn("Execution did not yield any table sets.");
                Logger.Error("No data will be exported.");
                return new List<Table>(0);
            }

            var tableSet = MergeBuckets(buckets);

            return tableSet;
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

            var emptyTableSetsResults = result.Where(executionResult => executionResult.TableSet.Count == 0);

            foreach (var executionResult in emptyTableSetsResults)
            {
                var message =
                    $"Execution result for {executionResult.Database.DatabaseName} in {executionResult.Database.ServerName} contains an empty table set.";
                Logger.Warn(message);
            }
        }

        private static ICollection<Table>[] MapToBuckets(ICollection<ExecutionResult> executionResults)
        {
            var maxTableCount = executionResults.Select(e => e.TableSet.Count).Max();
            var executionResultsCount = executionResults.Count;

            var queryTables = new ICollection<Table>[maxTableCount];

            for (var i = 0; i < queryTables.Length; i++)
            {
                queryTables[i] = new List<Table>(executionResultsCount);
            }

            var messageTables = new List<Table>(executionResultsCount);

            if (!DoMap(queryTables, messageTables, executionResults))
                return null;

            var buckets = queryTables.Concat(new[] {messageTables}).ToArray();

            if (!BucketsHaveIdenticalTableColumns(buckets))
            {
                return null;
            }

            return buckets;
        }

        private static bool DoMap(ICollection<Table>[] queryTables, ICollection<Table> messageTables, ICollection<ExecutionResult> executionResults)
        {
            foreach (var executionResult in executionResults)
            {
                var i = 0;

                foreach (var table in executionResult.TableSet)
                {
                    queryTables[i++].Add(table);
                }

                if (i == 0)
                {
                    // warn
                    LogTableComparisonWarning(executionResult, "Results can be merged although one of them has no table.");
                }
                else if (i != queryTables.Length)
                {
                    // abort
                    LogTableComparisonWarning(executionResult,
                        $"Results cannot be merged. They have different number of tables : {i} (should be {queryTables.Length}).");
                    return false;
                }

                if (executionResult.InformationMessages.HasValue)
                {
                    messageTables.Add(executionResult.InformationMessages.Value);
                }
            }

            return true;
        }
        
        private static bool BucketsHaveIdenticalTableColumns(ICollection<Table>[] buckets)
        {
            for (var i = 0; i < buckets.Length; i++)
            {
                var firstTable = buckets[i].First();
                var allTablesAreIdentical = buckets[i].All(x => firstTable.HasIdenticalColumns(x));

                if (allTablesAreIdentical)
                {
                    continue;
                }

                Logger.Warn($"Tables are not identical. Tables at index #{i} have different column sets.");

                return false;
            }

            return true;
        }
        
        private static List<Table> MergeBuckets(ICollection<Table>[] buckets)
        {
            var tableSet = new List<Table>(buckets.Length);

            foreach (var bucket in buckets)
            {
                if (bucket.Count == 0)
                {
                    Logger.Trace("Bucket is empty.");
                    continue;
                }

                Logger.Trace($"Bucket contains {bucket.Count} tables.");
                var firstTable = bucket.First();
                var columns = firstTable.Columns;
                var tableId = firstTable.Id.StartsWith("__", StringComparison.InvariantCulture) ? firstTable.Id : null;
                var rows = bucket.SelectMany(table => table.Rows).ToList();
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

            return tableSet;
        }
        
        private static void LogTableComparisonWarning(ExecutionResult result, string specificMessage)
        {
            Logger.Warn($"Tables are not all identical. See {result.Database.DatabaseName} in {result.Database.ServerName}. {specificMessage}");
        }
    }
}
