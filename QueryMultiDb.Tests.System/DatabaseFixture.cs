using System;
using System.Data.SqlClient;

namespace QueryMultiDb.Tests.System
{
    public class DatabaseFixture : IDisposable
    {
        private static readonly bool IsAppVeyor = Environment.GetEnvironmentVariable("Appveyor")?.ToUpperInvariant() == "TRUE";

        public static string ConnectionString => IsAppVeyor
            ? @"Data Source=.\SQL2017;Database=tempdb;Integrated Security=True"
            : @"Data Source=.;Initial Catalog=tempdb;Integrated Security=True";

        public static string OneTarget => IsAppVeyor
            ? @"{ ""DatabaseList"": [ { ""ServerName"": "".\SQL2017"", ""DatabaseName"": ""tempdb"" } ]}"
            : @"{ ""DatabaseList"": [ { ""ServerName"": ""localhost"", ""DatabaseName"": ""tempdb"" } ]}";
        
        public static string TwoTargets => IsAppVeyor
            ? @"{ ""DatabaseList"": [ { ""ServerName"": "".\SQL2017"", ""DatabaseName"": ""tempdb"" }, { ""ServerName"": "".\SQL2017"", ""DatabaseName"": ""tempdb"" } ]}"
            : @"{ ""DatabaseList"": [ { ""ServerName"": ""localhost"", ""DatabaseName"": ""tempdb"" }, { ""ServerName"": ""localhost"", ""DatabaseName"": ""tempdb"" } ]}";

        public static string TestTableSelectQuery = "SELECT * FROM TestTableOne;";
        private static string TestTableDropQuery = "DROP TABLE IF EXISTS TestTableOne;";
        
        public DatabaseFixture()
        {
            lock (ConnectionString)
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = SystemTestsHelpers.GetResource("QueryMultiDb.Tests.System.SqlResources.DatabaseFixture.sql");
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

        }

        public void Dispose()
        {
            lock (ConnectionString)
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = TestTableDropQuery;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
