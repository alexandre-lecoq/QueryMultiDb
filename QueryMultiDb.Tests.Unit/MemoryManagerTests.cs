using System;
using System.Globalization;
using Xunit;

namespace QueryMultiDb.Tests.Unit
{
    public class MemoryManagerTests
    {
        [Fact]
        public void Clean_DoesNotThrow()
        {
            MemoryManager.Clean();
            Assert.True(true);
        }

        [Theory]
        [InlineData(0, 0, "0 bytes")]
        [InlineData(1, 0, "1 bytes")]
        [InlineData(10, 0, "10 bytes")]
        [InlineData(10000, 0, "10 KB")]
        [InlineData(10000000, 0, "10 MB")]
        [InlineData(long.MaxValue, 0, "8 EB")]
        [InlineData(0, 3, "0.000 bytes")]
        [InlineData(1, 3, "1.000 bytes")]
        [InlineData(10, 3, "10.000 bytes")]
        [InlineData(1022, 3, "0.998 KB")]
        [InlineData(10000, 3, "9.766 KB")]
        [InlineData(10000000, 3, "9.537 MB")]
        [InlineData(long.MaxValue, 3, "8.000 EB")]
        [InlineData(-10000, 3, "-9.77 KB")]
        [InlineData(long.MinValue, 3, "-8.00 EB")]
        [InlineData(long.MaxValue, 28, "7.9999999999999999991326382620 EB")]
        public void ToSuffixedSizeString_CorrectConvertion(long value, int decimalPlaces, string expectedResult)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            var actualResult = value.ToSuffixedSizeString(decimalPlaces);
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(29)]
        public void ToSuffixedSizeString_InvalidDecimalThrowsException(int decimalPlaces)
        {
            const long value = 1000;
            Assert.Throws<ArgumentOutOfRangeException>(() => value.ToSuffixedSizeString(decimalPlaces));
        }
    }
}
