using System;
using System.Threading;

namespace QueryMultiDb
{
    public class Log
    {
        private static int _sequenceNumber;

        public int Id { get; }
        public string Server { get; }
        public string Database { get; }
        public int ThreadId { get; }
        public DateTime Date { get; }
        public string Level { get; }
        public string Message { get; }
        public Exception Exception { get; }

        public Log(string server, string database, string level, string message, Exception exception)
        {
            // This code is thread-friendly. It creates a weirdly increasing id (sometimes).
            // What matters is that only 2 differents thread might get the same sequence number.
            // That's what we want. Enough for us.
            Id = _sequenceNumber += 3;
            Date = DateTime.UtcNow;
            ThreadId = Thread.CurrentThread.ManagedThreadId;
            Server = server;
            Database = database;
            Level = level;
            Message = message;
            Exception = exception;
        }

        public override string ToString()
        {
            return $"{Level} : {Message}";
        }
    }
}
