using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryMultiDb
{
    public class ExecutionResult
    {
        public string ServerName { get; }

        public string DatabaseName { get; }

        public IList<Table> TableSet { get; }

        public ExecutionResult(string serverName, string databaseName, IList<Table> tableSet)
        {
            ServerName = serverName;
            DatabaseName = databaseName;
            TableSet = tableSet;
        }

        public override string ToString()
        {
            var tableCount = TableSet.Count;
            var totalRowCount = TableSet.Sum(table => table.Rows.Count);

            return
                $"ServerName = \"{ServerName}\" ; DatabaseName = \"{DatabaseName}\" ; Total row count = {totalRowCount} ; Table count = {tableCount}";
        }

        public bool HasIdenticalTableAndColumns(ExecutionResult other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (TableSet == null && other.TableSet == null)
            {
                return true;
            }

            if (TableSet == null || other.TableSet == null)
            {
                return false;
            }
 
            if (TableSet.Count != other.TableSet.Count)
            {
                return false;
            }

            for (var i = 0; i < TableSet.Count; i++)
            {
                var thisTable = TableSet[i];
                var otherTable = other.TableSet[i];

                var isIdentical = thisTable.HasIdenticalColumns(otherTable);

                if (!isIdentical)
                {
                    return false;
                }
            }

            return true;
        }
    }
}