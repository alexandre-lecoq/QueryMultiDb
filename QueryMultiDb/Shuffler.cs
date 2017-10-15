using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace QueryMultiDb
{
    /// <summary>
    /// Implements Fisher–Yates shuffle. Also known as Knuth shuffle.
    /// </summary>
    public static class Shuffler
    {
        /// <summary>
        /// The cryptographically secure random number generator.
        /// </summary>
        /// <remarks>It provides better random numbers than System.Random's substractive pseudo-random number generator.</remarks>
        private static readonly RandomNumberGenerator RandomNumberGenerator = RandomNumberGenerator.Create();

        /// <summary>
        /// Shuffles the given array.
        /// </summary>
        /// <param name="array">The array to shuffle.</param>
        public static void ShuffleArray<T>(T[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Array cannot be null.");
            }

            if (array.Length < 2)
            {
                return;
            }

            for (var i = 0; i < array.Length; i++)
            {
                var j = GetRandomNumber(i, array.Length);
                Swap(array, i, j);
            }
        }

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to minValue and less than maxValue;
        /// that is, the range of return values includes minValue but not maxValue. 
        /// If minValue equals maxValue, minValue is returned.
        /// </returns>
        public static int GetRandomNumber(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentException("minValue parameter cannot be greater than maxValue parameter.");
            }

            if (minValue == maxValue)
            {
                return minValue;
            }

            var difference = (long)maxValue - minValue;

            if (difference == 1)
            {
                return minValue;
            }

            int r;

            do
            {
                var b = new byte[4];
                RandomNumberGenerator.GetBytes(b);
                var i = BitConverter.ToUInt32(b, 0);
                var d = (double)i / 0xffffffff;
                r = (int)(minValue + (d * difference));
            }
            while (r == maxValue);

            return r;
        }

        /// <summary>
        /// Swap two elements in an array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="i">The index of the first element.</param>
        /// <param name="j">The index of the second element.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Swap<T>(T[] array, int i, int j)
        {
            var tmp = array[i];
            array[i] = array[j];
            array[j] = tmp;
        }
    }
}
