using System;

namespace QueryMultiDb
{
    public class Database
    {
        public string ServerName { get; }

        public string DatabaseName { get; }

        public string ExtraValue1 { get; }

        public string ExtraValue2 { get; }

        public string ExtraValue3 { get; }

        public string ExtraValue4 { get; }

        public string ExtraValue5 { get; }

        public string ExtraValue6 { get; }

        public Database(string serverName, string databaseName, string extraValue1, string extraValue2, string extraValue3, string extraValue4, string extraValue5, string extraValue6)
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
            ExtraValue1 = extraValue1 ?? string.Empty;
            ExtraValue2 = extraValue2 ?? string.Empty;
            ExtraValue3 = extraValue3 ?? string.Empty;
            ExtraValue4 = extraValue4 ?? string.Empty;
            ExtraValue5 = extraValue5 ?? string.Empty;
            ExtraValue6 = extraValue6 ?? string.Empty;
        }

        public override string ToString()
        {
            return $"ServerName = \"{ServerName}\" ; DatabaseName = \"{DatabaseName}\"";
        }

        public string ToLogPrefix()
        {
            return $"{ServerName}/{DatabaseName} ;";
        }
    }
}