using System;
using System.Diagnostics;
using NLog;
// ReSharper disable FormatStringProblem

namespace QueryMultiDb
{
    public static class MemoryManager
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly string[] SizeSuffixes =
        {
            "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"
        };

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

        private static string ToSuffixedSizeString(this long value, int decimalPlaces = 2)
        {
            if (decimalPlaces < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(decimalPlaces));
            }

            if (value < 0)
            {
                return "-" + ToSuffixedSizeString(-value);
            }

            if (value == 0)
            {
                return string.Format("{0:n" + decimalPlaces + "} bytes", 0);
            }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            var mag = (int) Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            var adjustedSize = (decimal) value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", adjustedSize, SizeSuffixes[mag]);
        }

    }
}
