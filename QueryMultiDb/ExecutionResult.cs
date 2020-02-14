using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryMultiDb
{
    public class ExecutionResult
    {
        public Database Database { get; }

        public IList<Table> TableSet { get; }

        public Table? InformationMessages { get; }

        public ExecutionResult(Database database, IList<Table> tableSet, Table? informationMessages)
        {
            Database = database ?? throw new ArgumentNullException(nameof(database));
            TableSet = tableSet ?? throw new ArgumentNullException(nameof(tableSet));
            InformationMessages = informationMessages;
        }

        public override string ToString()
        {
            var tableCount = TableSet.Count;
            var totalRowCount = TableSet.Sum(table => table.Rows.Count);

            return $"{Database} ; Total row count = {totalRowCount} ; Table count = {tableCount}";
        }
    }
}