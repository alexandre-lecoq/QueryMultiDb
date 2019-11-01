using System;

namespace QueryMultiDb.DataMerger
{
    public static class DataMergerFactory
    {
        public static IDataMerger GetDataMerger(DataMergerType type)
        {
            switch (type)
            {
                case DataMergerType.Strict:
                    return new StrictDataMerger();
                case DataMergerType.Conservative:
                    return new ConservativeDataMerger();
                case DataMergerType.Null:
                    return new NullDataMerger();
                case DataMergerType.Opportunist:
                    return new OpportunistDataMerger();
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
