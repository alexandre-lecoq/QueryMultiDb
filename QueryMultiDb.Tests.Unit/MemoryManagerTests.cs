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
    }
}
