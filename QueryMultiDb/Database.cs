namespace QueryMultiDb
{
    public class Database
    {
        public string ServerName { get; }

        public string DatabaseName { get; }

        public Database(string serverName, string databaseName)
        {
            ServerName = serverName;
            DatabaseName = databaseName;
        }

        public override string ToString()
        {
            return $"ServerName = \"{ServerName}\" ; DatabaseName = \"{DatabaseName}\"";
        }
    }
}