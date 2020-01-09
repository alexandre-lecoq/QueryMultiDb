using System.Collections.Generic;

namespace QueryMultiDb.DataMerger
{
    public interface IDataMerger
    {
        string Name { get; }

        ICollection<Table> MergeResults(ICollection<ExecutionResult> executionResults);
    }
}
