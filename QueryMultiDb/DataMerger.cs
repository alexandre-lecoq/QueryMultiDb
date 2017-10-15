using System.Collections.Generic;
using System.Linq;

namespace QueryMultiDb
{
    public static class DataMerger
    {
        public static ICollection<Table> MergeResults(ICollection<ExecutionResult> result)
        {
            if (result == null)
            {
                return null;
            }

            if (result.Count == 0)
            {
                return new List<Table>(0);
            }

            var tableCounts = result.Select(executionResult => executionResult.TableSet.Count).ToList();
            var maxTableCount = tableCounts.Max();
            var minTableCount = tableCounts.Min();

            if (minTableCount != maxTableCount)
            {
                Logger.Instance.Warn($"Executions yielded different number of tables. Minimum : {minTableCount} ; Maximum : {maxTableCount}");
            }

            var tableSet = new List<Table>(maxTableCount);

            for (var tableIndex = 0; tableIndex < maxTableCount; tableIndex++)
            {
                var computedColumns = FindColumnSet(result, tableIndex);

                if (computedColumns != null)
                {
                    var builtInColumnSet = new List<TableColumn>(9);
                    builtInColumnSet.Add(new TableColumn("_ServerName", typeof(string)));

                    if (Parameters.Instance.IncludeIP)
                    {
                        builtInColumnSet.Add(new TableColumn("_ServerIp", typeof(string)));
                    }

                    builtInColumnSet.Add(new TableColumn("_DatabaseName", typeof(string)));

                    var destinationColumnSet = new TableColumn[builtInColumnSet.Count + computedColumns.Length];
                    builtInColumnSet.CopyTo(destinationColumnSet, 0);
                    computedColumns.CopyTo(destinationColumnSet, builtInColumnSet.Count);
                    var rows = ComputeRowSet(result, tableIndex);
                    var destinationTable = new Table(destinationColumnSet, rows);
                    tableSet.Add(destinationTable);
                }
                else
                {
                    Logger.Instance.Error($"No column set could be computed for table set {tableIndex} ; Table will be skipped.");
                }
            }

            return tableSet;
        }

        private static ICollection<TableRow> ComputeRowSet(ICollection<ExecutionResult> result, int tableIndex)
        {
            var tableRows = new List<TableRow>();

            foreach (var executionResult in result)
            {
                var sourceTable = executionResult.TableSet.ElementAt(tableIndex);

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

        private static TableColumn[] FindColumnSet(IEnumerable<ExecutionResult> result, int tableIndex)
        {
            TableColumn[] previousColumns = null;

            foreach (var executionResult in result)
            {
                var sourceTable = executionResult.TableSet.ElementAt(tableIndex);

                if (previousColumns != null)
                {
                    if (!sourceTable.Columns.IsIdentical(previousColumns))
                    {
                        Logger.Instance.Error("Not all column sets are identical.");
                        return null;
                    }
                }

                previousColumns = sourceTable.Columns;
            }

            return previousColumns;
        }
    }
}
