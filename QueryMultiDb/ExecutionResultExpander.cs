using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryMultiDb
{
    public static class ExecutionResultExpander
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static ICollection<ExecutionResult> ComputeBuildInColumns(ICollection<ExecutionResult> results)
        {
            if (results == null)
            {
                throw new ArgumentNullException(nameof(results), "Parameter cannot be null.");
            }
            
            if (results.Count == 0)
            {
                return new List<ExecutionResult>(0);
            }

            var processedResults = new List<ExecutionResult>(results.Count);

            foreach (var result in results)
            {
                var tableSet = result.TableSet.Select(inputTable => ComputeExpandedTable(result.Database, inputTable)).ToList();
                var executionResult = new ExecutionResult(result.Database, tableSet);
                processedResults.Add(executionResult);
            }

            return processedResults;
        }

        private static Table ComputeExpandedTable(Database database, Table inputTable)
        {
            var columns = ComputeColumnSet(inputTable);
            var rows = ComputeRowSet(database, inputTable);
            // Although columns and rows changes and the ID might be a hash, we don't update it.
            // It doesn't appear to be needed today, and the ID could also be special table ID.
            // Anyway, next time a table is created from this one without propagating the ID, a new one will be computed.
            var outputTable = new Table(columns, rows, inputTable.Id);

            return outputTable;
        }
        
        private static TableColumn[] ComputeColumnSet(Table inputTable)
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

            var computedColumns = inputTable.Columns;
            var destinationColumnSet = new TableColumn[builtInColumnSet.Count + computedColumns.Length];
            builtInColumnSet.CopyTo(destinationColumnSet, 0);
            computedColumns.CopyTo(destinationColumnSet, builtInColumnSet.Count);

            return destinationColumnSet;
        }

        private static ICollection<TableRow> ComputeRowSet(Database database, Table inputTable)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }
            
            var tableRows = new List<TableRow>(inputTable.Rows.Count);

            foreach (var tableRow in inputTable.Rows)
            {
                var builtInItems = new List<object>(10);

                if (Parameters.Instance.ShowServerName)
                {
                    builtInItems.Add(database.ServerName);
                }

                if (Parameters.Instance.ShowIpAddress)
                {
                    var ip = ResolveServerName(database.ServerName);
                    builtInItems.Add(ip);
                }

                if (Parameters.Instance.ShowDatabaseName)
                {
                    builtInItems.Add(database.DatabaseName);
                }

                if (Parameters.Instance.ShowExtraColumns)
                {
                    if (!Parameters.Instance.Targets.EmptyExtraValues[0])
                    {
                        builtInItems.Add(database.ExtraValue1);
                    }

                    if (!Parameters.Instance.Targets.EmptyExtraValues[1])
                    {
                        builtInItems.Add(database.ExtraValue2);
                    }

                    if (!Parameters.Instance.Targets.EmptyExtraValues[2])
                    {
                        builtInItems.Add(database.ExtraValue3);
                    }

                    if (!Parameters.Instance.Targets.EmptyExtraValues[3])
                    {
                        builtInItems.Add(database.ExtraValue4);
                    }

                    if (!Parameters.Instance.Targets.EmptyExtraValues[4])
                    {
                        builtInItems.Add(database.ExtraValue5);
                    }

                    if (!Parameters.Instance.Targets.EmptyExtraValues[5])
                    {
                        builtInItems.Add(database.ExtraValue6);
                    }
                }

                var columnCount = tableRow.ItemArray.Length;
                var items = new object[builtInItems.Count + columnCount];
                builtInItems.CopyTo(items, 0);
                tableRow.ItemArray.CopyTo(items, builtInItems.Count);
                var newRow = new TableRow(items);
                tableRows.Add(newRow);
            }

            return tableRows;
        }

        private static string ResolveServerName(string serverName)
        {
            var (host, instance) = ParserSqlServerInstance(serverName);
            var ip = DnsResolverWithCache.Instance.Resolve(host);
            var ipString = ip?.ToString() ?? string.Empty;
            var resolvedServerName = instance == null ? ipString : ipString + "\\" + instance;

            return resolvedServerName;
        }

        private static (string, string) ParserSqlServerInstance(string databaseServerName)
        {
            if (string.IsNullOrWhiteSpace(databaseServerName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(databaseServerName));
            }

            var entries = databaseServerName.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            var host = entries[0];
            var instance = databaseServerName.Length == host.Length
                ? null
                : databaseServerName.Remove(0, host.Length + 1);

            return (host, instance);
        }
    }
}
