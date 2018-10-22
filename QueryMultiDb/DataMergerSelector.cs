using System;
using System.Collections.Generic;

namespace QueryMultiDb
{
    public static class DataMergerSelector
    {
        public static Func<ICollection<ExecutionResult>, ICollection<Table>> GetDataMerger(DataMergerType type)
        {
            switch (type)
            {
                case DataMergerType.Default:
                    return DataMerger.MergeResults;

                default:
                    throw new NotSupportedException($"Unsupported type '{type}'.");
            }
        }
    }
}
