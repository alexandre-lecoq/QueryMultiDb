using System;
using System.Collections.Generic;
using System.Threading;
using NLog;
using NLog.Targets;

namespace QueryMultiDb
{
    public sealed class TableTarget : Target
    {
        private readonly List<TableRow> _logRows;

        public TableTarget()
        {
            var logColumns = new TableColumn[7];
            logColumns[0] = new TableColumn("Id", typeof(int));
            logColumns[1] = new TableColumn("Date", typeof(string));
            logColumns[2] = new TableColumn("ThreadId", typeof(int));
            logColumns[3] = new TableColumn("Level", typeof(string));
            logColumns[4] = new TableColumn("Logger", typeof(string));
            logColumns[5] = new TableColumn("Message", typeof(string));
            logColumns[6] = new TableColumn("Exception", typeof(string));

            _logRows = new List<TableRow>();
            var logTable = new Table(logColumns, _logRows, Table.LogsId);

            Logs = logTable;
            OptimizeBufferReuse = true;
        }

        public TableTarget(string name) : this()
        {
            Name = name;
        }

        public Table Logs { get; }

        protected override void Write(LogEventInfo logEvent)
        {
            if (logEvent == null)
            {
                throw new ArgumentNullException(nameof(logEvent));
            }

            var items = new object[7];
            items[0] = logEvent.SequenceID;
            items[1] = logEvent.TimeStamp.ToString("o");
            items[2] = Thread.CurrentThread.ManagedThreadId; // System.Environment.CurrentManagedThreadId;
            items[3] = logEvent.Level;
            items[4] = logEvent.LoggerName;
            items[5] = logEvent.Message;
            items[6] = logEvent.Exception?.ToString() ?? "";
            var tableRow = new TableRow(items);
            _logRows.Add(tableRow);
        }

    }
}
