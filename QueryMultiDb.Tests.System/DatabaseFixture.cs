using System;
using System.Data.SqlClient;

namespace QueryMultiDb.Tests.System
{
    public class DatabaseFixture : IDisposable
    {
        private static readonly bool IsAppVeyor = Environment.GetEnvironmentVariable("Appveyor")?.ToUpperInvariant() == "TRUE";
        private static readonly string ServerName = IsAppVeyor ? @".\SQL2017" : @"localhost";
        private static readonly string DatabaseName = "tempdb";
        private const string ServerNameTag = "<{ServerName}>";
        private const string DatabaseNameTag = "<{DatabaseName}>";

        public static string ConnectionString => $@"Data Source={ServerName};Database={DatabaseName};Integrated Security=True";

        public static string OneTarget =>
            $@"{{ ""DatabaseList"": [ {{ ""ServerName"": ""{ServerName}"", ""DatabaseName"": ""{DatabaseName}"" }} ]}}";

        public static string TwoTargets =>
            $@"{{ ""DatabaseList"": [ {{ ""ServerName"": ""{ServerName}"", ""DatabaseName"": ""{DatabaseName}"" }}, {{ ""ServerName"": ""{ServerName}"", ""DatabaseName"": ""{DatabaseName}"" }} ]}}";

        public static string TestTableSelectQuery = "SELECT * FROM TestTableOne;";
        private static string TestTableDropQuery = "DROP TABLE IF EXISTS TestTableOne;";

        public static string FormatTargets(string format)
        {
            var formatted = format.Replace(ServerNameTag, ServerName).Replace(DatabaseNameTag, DatabaseName);

            return formatted;
        }

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
