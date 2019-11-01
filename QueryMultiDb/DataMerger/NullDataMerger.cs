using System.Collections.Generic;

namespace QueryMultiDb.DataMerger
{
    public class NullDataMerger : DataMerger
    {
        public override string Name => "Null Data Merger";

        public override ICollection<Table> MergeResults(ICollection<ExecutionResult> result)
        {
            throw new System.NotImplementedException();
        }
    }
}
