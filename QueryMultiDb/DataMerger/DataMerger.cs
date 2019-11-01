using NLog;
using System.Collections.Generic;

namespace QueryMultiDb.DataMerger
{
    public abstract class DataMerger : IDataMerger
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public abstract string Name { get; }

        public abstract ICollection<Table> MergeResults(ICollection<ExecutionResult> result);
    }
}
