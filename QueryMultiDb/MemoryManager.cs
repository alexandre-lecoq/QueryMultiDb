using System;
using System.Diagnostics;
using NLog;
using QueryMultiDb.Common;

// ReSharper disable FormatStringProblem

namespace QueryMultiDb
{
    public static class MemoryManager
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Clean()
        {
            var bytesBeforeCollection = GC.GetTotalMemory(false);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var bytesAfterCollection = GC.GetTotalMemory(true);
            stopwatch.Stop();
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            var message = $"Garbage collection done in {elapsedMilliseconds}ms. Before : {bytesBeforeCollection.ToSuffixedSizeString()}. After : {bytesAfterCollection.ToSuffixedSizeString()}.";
            Logger.Info(message);
        }
    }
}
