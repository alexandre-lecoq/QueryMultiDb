using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace QueryMultiDb
{
    public class Logger
    {
        private const string ErrorLevel = "Error";
        private const string WarnLevel = "Warn";
        private const string InfoLevel = "Info";

        private readonly List<Log> _logs;
        private readonly ConcurrentDictionary<int, List<Log>> _threadLogs;

        public ICollection<Log> Logs
        {
            get
            {
                lock (_logs)
                {
                    UnsynchronizedMerge();
                }

                return _logs;
            }  
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Suppreses beforefieldinit in static singleton.")]
        static Logger()
        {
        }

        private Logger()
        {
            _logs = new List<Log>();
            _threadLogs = new ConcurrentDictionary<int, List<Log>>();
        }

        public static Logger Instance { get; } = new Logger();

        public void Error(string message)
        {
            Error(message, null, null, null);
        }

        public void Error(string message, Exception exception)
        {
            Error(message, exception, null, null);
        }

        public void Error(string message, string server, string database)
        {
            Error(message, null, server, database);
        }

        public void Error(string message, Exception exception, string server, string database)
        {
            AddLog(ErrorLevel, server, database, message, exception);
        }
        
        public void Warn(string message)
        {
            Warn(message, null, null, null);
        }

        public void Warn(string message, Exception exception)
        {
            Warn(message, exception, null, null);
        }

        public void Warn(string message, string server, string database)
        {
            Warn(message, null, server, database);
        }
        
        public void Warn(string message, Exception exception, string server, string database)
        {
            AddLog(WarnLevel, server, database, message, exception);
        }
        
        public void Info(string message)
        {
            Info(message, null, null, null);
        }

        public void Info(string message, Exception exception)
        {
            Info(message, exception, null, null);
        }

        public void Info(string message, string server, string database)
        {
            Info(message, null, server, database);
        }

        public void Info(string message, Exception exception, string server, string database)
        {
            AddLog(InfoLevel, server, database, message, exception);
        }

        private void AddLog(string level, string server, string database, string message, Exception exception)
        {
            var log = new Log(server, database, level, message, exception);
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var threadLogs = _threadLogs.GetOrAdd(threadId, new List<Log>());
            threadLogs.Add(log);

            if (level == InfoLevel)
            {
                if (!Parameters.Instance.Quiet)
                {
                    Console.Out.WriteLine($"{log.Date:o} : {log.Level} : {log.Message}");
                }
            }
            else
            {
                Console.Error.WriteLine($"{log.Date:o} : {log.Level} : {log.Message} : {log.Exception}");
            }
        }
        
        private void UnsynchronizedMerge()
        {
            this.Info("Merging thread log collections to global log collection.");
            _logs.Clear();

            foreach (var threadLog in _threadLogs.Values)
            {
                if (threadLog != null)
                {
                    _logs.AddRange(threadLog);
                }
            }

            _logs.Sort(CompareLogsByDate);
        }

        private static int CompareLogsByDate(Log x, Log y)
        {
            if (x.Date < y.Date)
            {
                return -1;
            }

            if (x.Date > y.Date)
            {
                return 1;
            }

            if (x.Id < y.Id)
            {
                return -1;
            }

            if (x.Id > y.Id)
            {
                return 1;
            }

            return 0;
        }

        public override string ToString()
        {
            var acquired = Monitor.TryEnter(_logs);

            if (acquired)
            {
                try
                {
                    UnsynchronizedMerge();
                }
                finally
                {
                    Monitor.Exit(_logs);
                }
            }

            var totalLogCount = _logs.Count;
            var infoLogCount = _logs.Count(l => l.Level == InfoLevel);
            var warnLogCount = _logs.Count(l => l.Level == WarnLevel);
            var errorLogCount = _logs.Count(l => l.Level == ErrorLevel);

            return $"Total = {totalLogCount} ; Info = {infoLogCount} ; Warn = {warnLogCount} ; Errors = {errorLogCount} ; Synchronized = {acquired}";
        }
    }
}
