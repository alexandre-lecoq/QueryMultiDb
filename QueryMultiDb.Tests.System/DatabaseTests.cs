﻿using QueryMultiDb.Common;
using System.Data.SqlClient;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace QueryMultiDb.Tests.System
{
    public class DatabaseTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        private readonly ITestOutputHelper _output;

        public DatabaseTests(ITestOutputHelper output, DatabaseFixture fixture)
        {
            _fixture = fixture;
            _output = output;
        }

        [Fact]
        public void WorkingTestTableSelectQuery()
        {
            using (var connection = new SqlConnection(DatabaseFixture.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = DatabaseFixture.TestTableSelectQuery;
                var reader = command.ExecuteReader();
                _output.WriteLine($"HasRows : {reader.HasRows}");
                _output.WriteLine($"FieldCount : {reader.FieldCount}");
                reader.Close();
                reader.Dispose();
                connection.Close();
            }
        }

        [Fact]
        public void SimpleTestTableSelect()
        {
            var argumentStringBuilder = new QueryMultiDbArgumentStringBuilder();
            var systemRunOutput = SystemTestsHelpers.RunQueryMultiDbExecutionFromData(DatabaseFixture.TestTableSelectQuery, DatabaseFixture.OneTarget, argumentStringBuilder);
            _output.WriteLine(systemRunOutput.ToVerboseString());
            SystemTestsHelpers.AssertStandardSuccessConditions(systemRunOutput);
            var sheetCount = SystemTestsHelpers.AssertStandardExcelSuccessConditions(systemRunOutput);
        }

        [Fact]
        public void Test1()
        {
                var argumentStringBuilder = new QueryMultiDbArgumentStringBuilder();
                var query = SystemTestsHelpers.GetResource("QueryMultiDb.Tests.System.SqlResources.Test1.sql");
                var systemRunOutput = SystemTestsHelpers.RunQueryMultiDbExecutionFromData(query, DatabaseFixture.OneTarget, argumentStringBuilder);
                _output.WriteLine(systemRunOutput.ToVerboseString());
                SystemTestsHelpers.AssertStandardSuccessConditions(systemRunOutput);
                var sheetCount = SystemTestsHelpers.AssertStandardExcelSuccessConditions(systemRunOutput);

                Assert.DoesNotMatch("ERROR", systemRunOutput.StandardOutput);
                Assert.Equal(6, sheetCount);
                Assert.True(systemRunOutput.OutputFileContent.Length > 10000);
                Assert.True(systemRunOutput.OutputFileContent.Length < 20000);
        }

        [Fact]
        public void Test2()
        {
            var argumentStringBuilder = new QueryMultiDbArgumentStringBuilder();
            var query = SystemTestsHelpers.GetResource("QueryMultiDb.Tests.System.SqlResources.Test2.sql");
            var systemRunOutput = SystemTestsHelpers.RunQueryMultiDbExecutionFromData(query, DatabaseFixture.OneTarget, argumentStringBuilder);
            _output.WriteLine(systemRunOutput.ToVerboseString());
            SystemTestsHelpers.AssertStandardSuccessConditions(systemRunOutput);
            var sheetCount = SystemTestsHelpers.AssertStandardExcelSuccessConditions(systemRunOutput);

            Assert.DoesNotMatch("ERROR", systemRunOutput.StandardOutput);
            Assert.Equal(4, sheetCount);
            Assert.True(systemRunOutput.OutputFileContent.Length > 10000);
            Assert.True(systemRunOutput.OutputFileContent.Length < 20000);
        }

        [Fact]
        public void Test3()
        {
            var argumentStringBuilder = new QueryMultiDbArgumentStringBuilder();
            var query = SystemTestsHelpers.GetResource("QueryMultiDb.Tests.System.SqlResources.Test3.sql");
            var systemRunOutput = SystemTestsHelpers.RunQueryMultiDbExecutionFromData(query, DatabaseFixture.OneTarget, argumentStringBuilder);
            _output.WriteLine(systemRunOutput.ToVerboseString());
            SystemTestsHelpers.AssertStandardSuccessConditions(systemRunOutput);
            var sheetCount = SystemTestsHelpers.AssertStandardExcelSuccessConditions(systemRunOutput);

            Assert.DoesNotMatch("ERROR", systemRunOutput.StandardOutput);
            Assert.Equal(3, sheetCount);
            Assert.True(systemRunOutput.OutputFileContent.Length > 5000);
            Assert.True(systemRunOutput.OutputFileContent.Length < 10000);
        }

        [Fact]
        public void Test4()
        {
            var argumentStringBuilder = new QueryMultiDbArgumentStringBuilder();
            var query = SystemTestsHelpers.GetResource("QueryMultiDb.Tests.System.SqlResources.Test4.sql");
            var systemRunOutput = SystemTestsHelpers.RunQueryMultiDbExecutionFromData(query, DatabaseFixture.TwoTargets, argumentStringBuilder);
            _output.WriteLine(systemRunOutput.ToVerboseString());
            SystemTestsHelpers.AssertStandardSuccessConditions(systemRunOutput);
            var sheetCount = SystemTestsHelpers.AssertStandardExcelSuccessConditions(systemRunOutput);

            Assert.DoesNotMatch("ERROR", systemRunOutput.StandardOutput);
            Assert.Equal(4, sheetCount);
            Assert.True(systemRunOutput.OutputFileContent.Length > 8000);
            Assert.True(systemRunOutput.OutputFileContent.Length < 12000);
        }

        [Fact]
        public void Test5()
        {
            var argumentStringBuilder = new QueryMultiDbArgumentStringBuilder();
            var query = SystemTestsHelpers.GetResource("QueryMultiDb.Tests.System.SqlResources.Test5.sql");
            var systemRunOutput = SystemTestsHelpers.RunQueryMultiDbExecutionFromData(query, DatabaseFixture.OneTarget, argumentStringBuilder);
            _output.WriteLine(systemRunOutput.ToVerboseString());
            SystemTestsHelpers.AssertStandardSuccessConditions(systemRunOutput);
            var sheetCount = SystemTestsHelpers.AssertStandardExcelSuccessConditions(systemRunOutput);

            Assert.DoesNotMatch("ERROR", systemRunOutput.StandardOutput);
            Assert.Equal(3, sheetCount);
            Assert.True(systemRunOutput.OutputFileContent.Length > 5000);
            Assert.True(systemRunOutput.OutputFileContent.Length < 10000);
        }

        [Fact]
        public void Test6()
        {
            var argumentStringBuilder = new QueryMultiDbArgumentStringBuilder();
            var query = SystemTestsHelpers.GetResource("QueryMultiDb.Tests.System.SqlResources.Test6.sql");
            var systemRunOutput = SystemTestsHelpers.RunQueryMultiDbExecutionFromData(query, DatabaseFixture.OneTarget, argumentStringBuilder);
            _output.WriteLine(systemRunOutput.ToVerboseString());
            SystemTestsHelpers.AssertStandardSuccessConditions(systemRunOutput);
            var sheetCount = SystemTestsHelpers.AssertStandardExcelSuccessConditions(systemRunOutput);

            Assert.DoesNotMatch("ERROR", systemRunOutput.StandardOutput);
            Assert.Equal(3, sheetCount);
            Assert.True(systemRunOutput.OutputFileContent.Length > 5000);
            Assert.True(systemRunOutput.OutputFileContent.Length < 10000);
        }

        [Fact]
        public void Test7()
        {
            var argumentStringBuilder = new QueryMultiDbArgumentStringBuilder();
            var query = SystemTestsHelpers.GetResource("QueryMultiDb.Tests.System.SqlResources.Test7.sql");
            var systemRunOutput = SystemTestsHelpers.RunQueryMultiDbExecutionFromData(query, DatabaseFixture.OneTarget, argumentStringBuilder);
            _output.WriteLine(systemRunOutput.ToVerboseString());
            SystemTestsHelpers.AssertStandardSuccessConditions(systemRunOutput);
            var sheetCount = SystemTestsHelpers.AssertStandardExcelSuccessConditions(systemRunOutput);

            Assert.DoesNotMatch("ERROR", systemRunOutput.StandardOutput);
            Assert.Equal(5, sheetCount);
            Assert.True(systemRunOutput.OutputFileContent.Length > 10000);
            Assert.True(systemRunOutput.OutputFileContent.Length < 20000);
        }

        [Fact]
        public void Test8()
        {
            var argumentStringBuilder = new QueryMultiDbArgumentStringBuilder();
            var query = SystemTestsHelpers.GetResource("QueryMultiDb.Tests.System.SqlResources.Test8.sql");
            var systemRunOutput = SystemTestsHelpers.RunQueryMultiDbExecutionFromData(query, DatabaseFixture.TwoTargets, argumentStringBuilder);
            _output.WriteLine(systemRunOutput.ToVerboseString());
            SystemTestsHelpers.AssertStandardSuccessConditions(systemRunOutput);
            var sheetCount = SystemTestsHelpers.AssertStandardExcelSuccessConditions(systemRunOutput);

            Assert.DoesNotMatch("ERROR", systemRunOutput.StandardOutput);
            Assert.Equal(5, sheetCount);
            Assert.True(systemRunOutput.OutputFileContent.Length > 8000);
            Assert.True(systemRunOutput.OutputFileContent.Length < 15000);
        }

        [Fact]
        public void Test9()
        {
            var argumentStringBuilder = new QueryMultiDbArgumentStringBuilder();
            var query = SystemTestsHelpers.GetResource("QueryMultiDb.Tests.System.SqlResources.Test9.sql");
            var systemRunOutput = SystemTestsHelpers.RunQueryMultiDbExecutionFromData(query, DatabaseFixture.OneTarget, argumentStringBuilder);
            _output.WriteLine(systemRunOutput.ToVerboseString());
            SystemTestsHelpers.AssertStandardSuccessConditions(systemRunOutput);
            var sheetCount = SystemTestsHelpers.AssertStandardExcelSuccessConditions(systemRunOutput);

            Assert.DoesNotMatch("ERROR", systemRunOutput.StandardOutput);
            Assert.Equal(5, sheetCount);
            Assert.True(systemRunOutput.OutputFileContent.Length > 10000);
            Assert.True(systemRunOutput.OutputFileContent.Length < 20000);
        }
    }
}
