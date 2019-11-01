using System.Collections.Generic;

namespace QueryMultiDb.DataMerger
{
    public class OpportunistDataMerger : DataMerger
    {
        public override string Name => this.GetType().Name;

        public override ICollection<Table> MergeResults(ICollection<ExecutionResult> result)
        {
            throw new System.NotImplementedException();
        }
    }
}

