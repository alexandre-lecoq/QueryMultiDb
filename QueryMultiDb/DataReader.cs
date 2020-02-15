using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace QueryMultiDb
{
    public static class DataReader
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static int _concurrentExecutingQueries;
        private static int _maxConcurrentExecutingQueries;
        private static readonly object ConcurrentExecutionLock = new object();

        public static ICollection<ExecutionResult> GetQueryResults()
        {
            var resultSets = new List<ExecutionResult>();

            var progressReporter = new ProgressReporter("DataReader.GetQueryResults", Parameters.Instance.Targets.Databases.Count(), s => Console.Error.WriteLine(s));

            void AddDatabaseQueryResults(Database database)
            {
                var result = QueryDatabase(database);
                progressReporter.Increment();

                if (result == null)
                {
                    return;
                }

                // Lock is useless with foreach but useful with Parallel.ForEach.
				// We also don't mind the performance impact.
                lock (resultSets)
                {
                    resultSets.Add(result);
                }
            }

            if (Parameters.Instance.Sequential)
            {
                foreach (var database in Parameters.Instance.Targets.Databases)
                {
                    AddDatabaseQueryResults(database);
                }
            }
            else
            {
                var options = new ParallelOptions { MaxDegreeOfParallelism = Parameters.Instance.Parallelism };
                Parallel.ForEach(Parameters.Instance.Targets.Databases, options, AddDatabaseQueryResults);
            }

            progressReporter.Done();

            // ReSharper disable once InconsistentlySynchronizedField : Parallelization is already finished when this is reached.
            Logger.Info($"Maximum concurrent queries : {_maxConcurrentExecutingQueries} queries.");

            return resultSets;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Query string passed from user input on purpose.")]
        private static ExecutionResult QueryDatabase(Database database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            var titleAttribute = (AssemblyTitleAttribute) Assembly.GetExecutingAssembly()
                .GetCustomAttribute(typeof(AssemblyTitleAttribute));
            var applicationName = titleAttribute.Title;

            if (!string.IsNullOrWhiteSpace(Parameters.Instance.ApplicationName))
            {
                applicationName += " / " + Parameters.Instance.ApplicationName.Trim();
            }

            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = database.ServerName,
                InitialCatalog = database.DatabaseName,
                ConnectTimeout = Parameters.Instance.ConnectionTimeout,
                IntegratedSecurity = true,
                WorkstationID = Environment.MachineName,
                ApplicationName = applicationName,
                ApplicationIntent = ApplicationIntent.ReadWrite,
                Pooling = false
            };

            ExecutionResult result = null;

            var openStopwatch = new Stopwatch();
            var queryStopwatch = new Stopwatch();

            try
            {
                lock (ConcurrentExecutionLock)
                {
                    _concurrentExecutingQueries++;

                    if (_maxConcurrentExecutingQueries < _concurrentExecutingQueries)
                    {
                        _maxConcurrentExecutingQueries = _concurrentExecutingQueries;
                    }
                }

                var connectionString = connectionStringBuilder.ToString();
                Logger.Trace($"Connection string : '{connectionString}'");

                using (var connection = new SqlConnection(connectionString))
                {
                    openStopwatch.Start();
                    connection.Open();
                    openStopwatch.Stop();

                    queryStopwatch.Start();
                    result = GetExecutionResult(connection, database);
                    queryStopwatch.Stop();

                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                Logger.Error(ex, $"{database.ToLogPrefix()} {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"{database.ToLogPrefix()} {ex.Message}");
            }
            finally
            {
                openStopwatch.Stop();
                queryStopwatch.Stop();

                lock (ConcurrentExecutionLock)
                {
                    _concurrentExecutingQueries--;
                }
            }

            Logger.Debug($"{database.ToLogPrefix()} SQL connection : {openStopwatch.Elapsed.TotalMilliseconds.ToString(CultureInfo.InvariantCulture)} milliseconds.");
            Logger.Debug($"{database.ToLogPrefix()} SQL query : {queryStopwatch.Elapsed.TotalMilliseconds.ToString(CultureInfo.InvariantCulture)} milliseconds.");

            return result;
        }

        private static ExecutionResult GetExecutionResult(SqlConnection connection, Database database)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            var infoMessageRows = new List<TableRow>();

            void SqlInfoMessageEventHandler(object sender, SqlInfoMessageEventArgs arg)
            {
                ConnectionOnInfoMessage(infoMessageRows, arg);
            }

            connection.FireInfoMessageEventOnUserErrors = true;
            connection.InfoMessage += SqlInfoMessageEventHandler;

            using (var command = connection.CreateCommand())
            {
                command.CommandText = Parameters.Instance.Query;
                command.CommandTimeout = Parameters.Instance.CommandTimeout;

                using (var reader = command.ExecuteReader(CommandBehavior.SequentialAccess))
                {
                    var tableSet = new List<Table>();

                    if (Parameters.Instance.DiscardResults)
                    {
                        ReadResultDataAndDiscard(reader, tableSet, database);
                    }
                    else
                    {
                        ReadResultData(reader, tableSet, database);
                    }

                    reader.Close();

                    // If the number of records affected is -1, it means it is a SELECT statement.
                    if (reader.RecordsAffected != -1)
                    {
                        Logger.Info($"{database.ToLogPrefix()} Records affected by query : {reader.RecordsAffected}");
                    }

                    var infoMessageColumns = new TableColumn[6];
                    infoMessageColumns[0] = new TableColumn("Class", typeof(string));
                    infoMessageColumns[1] = new TableColumn("Number", typeof(string));
                    infoMessageColumns[2] = new TableColumn("State", typeof(string));
                    infoMessageColumns[3] = new TableColumn("Procedure", typeof(string));
                    infoMessageColumns[4] = new TableColumn("LineNumber", typeof(string));
                    infoMessageColumns[5] = new TableColumn("Message", typeof(string));
                    var informationMessageTable = new Table(infoMessageColumns, infoMessageRows, Table.InformationMessagesId);
                    var result = new ExecutionResult(database, tableSet, informationMessageTable);
                    connection.InfoMessage -= SqlInfoMessageEventHandler;

                    return result;
                }
            }
        }

        private static void ReadResultDataAndDiscard(IDataReader reader, ICollection<Table> tableSet, Database database)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (tableSet == null)
            {
                throw new ArgumentNullException(nameof(tableSet));
            }

            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            do
            {
                var fieldCount = reader.FieldCount;
                var rowCount = 0;

                while (reader.Read())
                {
                    rowCount++;
                }

                var columns = new TableColumn[2];
                columns[0] = new TableColumn("FieldCount", typeof(int));
                columns[1] = new TableColumn("RowCount", typeof(int));

                var itemArray = new object[] {fieldCount, rowCount};
                var row = new TableRow(itemArray);
                var rows = new List<TableRow> {row};

                var table = new Table(columns, rows);
                tableSet.Add(table);

                Logger.Debug($"{database.ToLogPrefix()} Rows in table : {rowCount} (discarded)");
            } while (reader.NextResult());
        }

        private static void ReadResultData(IDataReader reader, ICollection<Table> tableSet, Database database)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (tableSet == null)
            {
                throw new ArgumentNullException(nameof(tableSet));
            }

            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            do
            {
                var fieldCount = reader.FieldCount;

                if (fieldCount == 0)
                {
                    Logger.Warn($"{database.ToLogPrefix()} Table was skipped because it contained 0 column.");
                    continue;
                }

                var columns = new TableColumn[fieldCount];

                for (var i = 0; i < fieldCount; i++)
                {
                    var name = reader.GetName(i);
                    var type = reader.GetFieldType(i);
                    var column = new TableColumn(name, type);
                    columns[i] = column;
                }

                var rows = new List<TableRow>();

                while (reader.Read())
                {
                    var itemArray = new object[fieldCount];
                    reader.GetValues(itemArray);
                    var row = new TableRow(itemArray);
                    rows.Add(row);
                }

                var table = new Table(columns, rows);
                tableSet.Add(table);

                Logger.Debug($"{database.ToLogPrefix()} Rows in table : {table.Rows.Count}");
            } while (reader.NextResult());
        }

        private static void ConnectionOnInfoMessage(ICollection<TableRow> infoMessageRows, SqlInfoMessageEventArgs sqlInfoMessageEventArgs)
        {
            if (infoMessageRows == null)
            {
                throw new ArgumentNullException(nameof(infoMessageRows));
            }

            if (sqlInfoMessageEventArgs == null)
            {
                throw new ArgumentNullException(nameof(sqlInfoMessageEventArgs));
            }

            foreach (SqlError error in sqlInfoMessageEventArgs.Errors)
            {
                if (error.Class >= 11)
                {
                    Logger.Error($"Class {error.Class} message type {error.Number} from line {error.LineNumber}. See information messages.");
                }

                var items = new object[6];
                items[0] = error.Class;
                items[1] = error.Number;
                items[2] = error.State;
                items[3] = error.Procedure;
                items[4] = error.LineNumber;
                items[5] = error.Message;
                var row = new TableRow(items);
                infoMessageRows.Add(row);
            }
        }
    }
}
