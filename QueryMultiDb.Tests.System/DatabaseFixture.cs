using System;
using System.Data.SqlClient;

namespace QueryMultiDb.Tests.System
{
    public class DatabaseFixture : IDisposable
    {
        private static readonly bool IsAppVeyor = Environment.GetEnvironmentVariable("Appveyor")?.ToUpperInvariant() == "TRUE";

        public static readonly string DefaultConnectionString = IntegratedSecurityConnectionString;

        public static string SaConnectionString => IsAppVeyor
            ? @"Data Source=(local)\SQL2017;Database=tempdb;User ID=sa;Password=Password12!"
            : "Data Source=.;Initial Catalog=tempdb;User ID=sa;Password=xxxxxxxxx";

        public static string IntegratedSecurityConnectionString => IsAppVeyor
            ? @"Data Source=(local)\SQL2017;Database=tempdb;Integrated Security=True"
            : "Data Source=.;Initial Catalog=tempdb;Integrated Security=True";

        public static string OneTarget => IsAppVeyor
            ? @"{ ""DatabaseList"": [ { ""ServerName"": ""(local)\SQL2017"", ""DatabaseName"": ""tempdb"" } ]}"
            : @"{ ""DatabaseList"": [ { ""ServerName"": ""localhost"", ""DatabaseName"": ""tempdb"" } ]}";
        
        public static string TwoTargets => IsAppVeyor
            ? @"{ ""DatabaseList"": [ { ""ServerName"": ""(local)\SQL2017"", ""DatabaseName"": ""tempdb"" }, { ""ServerName"": ""(local)\SQL2017"", ""DatabaseName"": ""tempdb"" } ]}"
            : @"{ ""DatabaseList"": [ { ""ServerName"": ""localhost"", ""DatabaseName"": ""tempdb"" }, { ""ServerName"": ""localhost"", ""DatabaseName"": ""tempdb"" } ]}";

        public static string TestTableSelectQuery = "SELECT * FROM TestTableOne;";
        private static string TestTableDropQuery = "DROP TABLE IF EXISTS TestTableOne;";
        
        public DatabaseFixture()
        {
            lock (DefaultConnectionString)
            {
                using (var connection = new SqlConnection(DefaultConnectionString))
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
            lock (DefaultConnectionString)
            {
                using (var connection = new SqlConnection(DefaultConnectionString))
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
