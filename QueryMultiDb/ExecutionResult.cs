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
    }
}