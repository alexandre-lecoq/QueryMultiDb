using System.Collections.Generic;
using System.Linq;

namespace QueryMultiDb
{
    public class ExecutionResult
    {
        public string ServerName { get; }

        public string DatabaseName { get; }

        public ICollection<Table> TableSet { get; }

        public ExecutionResult(string serverName, string databaseName, ICollection<Table> tableSet)
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
            if (this.TableSet == null && other.TableSet == null)
            {
                return true;
            }

            if (this.TableSet == null || other.TableSet == null)
            {
                return false;
            }
 
            if (this.TableSet.Count != other.TableSet.Count)
            {
                return false;
            }

            for (var i = 0; i < this.TableSet.Count; i++)
            {
                var thisTable = this.TableSet.ElementAt(i);
                var otherTable = other.TableSet.ElementAt(i);

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