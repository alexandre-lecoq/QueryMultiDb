﻿namespace QueryMultiDb.Common
{
    using System;

    public static class Extensions
    {
        private static readonly string[] SizeSuffixes =
        {
            "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"
        };
        
        public static string ToSuffixedSizeString(this long value, int decimalPlaces = 2)
        {
            if (decimalPlaces < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(decimalPlaces));
            }

            if (decimalPlaces > 28)
            {
                throw new ArgumentOutOfRangeException(nameof(decimalPlaces));
            }

            if (value == long.MinValue)
            {
                return "-" + ToSuffixedSizeString(long.MaxValue);
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
