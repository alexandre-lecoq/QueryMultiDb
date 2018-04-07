using System;

namespace QueryMultiDb
{
    public class Database
    {
        public string ServerName { get; }

        public string DatabaseName { get; }

        public Database(string serverName, string databaseName)
        {
            if (string.IsNullOrWhiteSpace(serverName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serverName));
            }

            if (string.IsNullOrWhiteSpace(databaseName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(databaseName));
            }

            ServerName = serverName;
            DatabaseName = databaseName;
        }

        public override string ToString()
        {
            return $"ServerName = \"{ServerName}\" ; DatabaseName = \"{DatabaseName}\"";
        }
    }
}