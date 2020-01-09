using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryMultiDb.DataMerger
{
    public class NullDataMerger : DataMerger
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override string Name => "Null Data Merger";

        public override ICollection<Table> MergeResults(ICollection<ExecutionResult> executionResults)
        {
            if (executionResults == null)
            {
                throw new ArgumentNullException(nameof(executionResults), "Parameter cannot be null.");
            }

            if (executionResults.Count == 0)
            {
                Logger.Warn("Execution did not yield any results.");
                Logger.Error("No data will be exported.");
                return new List<Table>(0);
            }

            var tableCount = executionResults.Sum(r => r.TableSet.Count);
            var tableSet = new List<Table>(tableCount);
            tableSet.AddRange(executionResults.SelectMany(executionResult => executionResult.TableSet));

            return tableSet;
        }
    }
}
