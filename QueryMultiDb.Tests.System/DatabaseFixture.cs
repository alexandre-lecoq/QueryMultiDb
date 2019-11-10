using System;
using System.Data.SqlClient;

namespace QueryMultiDb.Tests.System
{
    public sealed class DatabaseFixture : IDisposable
    {
        private static readonly bool IsAppVeyor = Environment.GetEnvironmentVariable("Appveyor")?.ToUpperInvariant() == "TRUE";
        private static readonly string ServerName = IsAppVeyor ? @".\SQL2017" : @"localhost";
        private static readonly string DatabaseName = "tempdb";
        private const string ServerNameTag = "<{ServerName}>";
        private const string DatabaseNameTag = "<{DatabaseName}>";

        public static readonly string ConnectionString = $@"Data Source={ServerName};Database={DatabaseName};Integrated Security=True";

        public static string OneTarget =>
            $@"{{ ""DatabaseList"": [ {{ ""ServerName"": ""{EscapeJsonString(ServerName)}"", ""DatabaseName"": ""{DatabaseName}"" }} ]}}";

        public static string TwoTargets =>
            $@"{{ ""DatabaseList"": [ {{ ""ServerName"": ""{EscapeJsonString(ServerName)}"", ""DatabaseName"": ""{DatabaseName}"" }}, {{ ""ServerName"": ""{EscapeJsonString(ServerName)}"", ""DatabaseName"": ""{DatabaseName}"" }} ]}}";

        public const string TestTableSelectQuery = "SELECT * FROM TestTableOne;";

        private const string TestTableDropQuery = "DROP TABLE IF EXISTS TestTableOne;";

        public static string FormatTargets(string format)
        {
            var serverName = Escape(ServerName);
            var databaseName = Escape(DatabaseName);
            var formatted = format.Replace(ServerNameTag, serverName).Replace(DatabaseNameTag, databaseName);

            return formatted;
        }
        
        private static string Escape(string str)
        {
            return EscapeJsonString(EscapeDoubleQuotes(str));
        }

        private static string EscapeDoubleQuotes(string str)
        {
            return str.Replace("\"", "\"\"");
        }

        private static string EscapeJsonString(string str)
        {
            return str.Replace("\\", "\\\\");
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
