using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;

namespace QueryMultiDb
{
    public static class DataReader
    {
        private static int _concurrentExecutingQueries;
        private static int _maxConcurrentExecutingQueries;
        private static readonly object ConcurrentExecutionLock = new object();

        public static ICollection<ExecutionResult> GetQueryResults()
        {
            var resultSets = new List<ExecutionResult>();

            if (Parameters.Instance.Sequential)
            {
                foreach (var database in Parameters.Instance.Targets)
                {
                    var result = QueryDatabase(database);

                    if (result == null)
                    {
                        continue;
                    }

                    resultSets.Add(result);
                }
            }
            else
            {
                var options = new ParallelOptions { MaxDegreeOfParallelism = Parameters.Instance.Parallelism };

                Parallel.ForEach(Parameters.Instance.Targets, options, (database) =>
                {
                    var result = QueryDatabase(database);

                    if (result == null)
                    {
                        return;
                    }

                    lock (resultSets)
                    {
                        resultSets.Add(result);
                    }
                });
            }

            Logger.Instance.Info($"Maximum concurrent queries : {_maxConcurrentExecutingQueries} queries.");

            return resultSets;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Query string passed from user input on purpose.")]
        private static ExecutionResult QueryDatabase(Database database)
        {
            var titleAttribute = (AssemblyTitleAttribute) Assembly.GetExecutingAssembly()
                .GetCustomAttribute(typeof(AssemblyTitleAttribute));

            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = database.ServerName,
                InitialCatalog = database.DatabaseName,
                ConnectTimeout = Parameters.Instance.ConnectionTimeout,

                IntegratedSecurity = true,
                WorkstationID = Environment.MachineName,
                ApplicationName = titleAttribute.Title,

                ApplicationIntent = ApplicationIntent.ReadWrite,
                NetworkLibrary = "dbmssocn",
                Pooling = false,
                Authentication = SqlAuthenticationMethod.NotSpecified
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

                using (var connection = new SqlConnection(connectionStringBuilder.ToString()))
                {
                    openStopwatch.Start();
                    connection.Open();
                    openStopwatch.Stop();

                    queryStopwatch.Start();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = Parameters.Instance.Query;
                        command.CommandTimeout = Parameters.Instance.CommandTimeout;

                        using (var reader = command.ExecuteReader())
                        {
                            result = GetExecutionResult(reader, database);
                        }
                    }

                    queryStopwatch.Stop();

                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                Logger.Instance.Error(ex.Message, ex, database.ServerName, database.DatabaseName);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.Message, ex, database.ServerName, database.DatabaseName);
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

            Logger.Instance.Info($"SQL connection : {openStopwatch.Elapsed.TotalMilliseconds.ToString(CultureInfo.InvariantCulture)} milliseconds.",
                database.ServerName,
                database.DatabaseName);
            Logger.Instance.Info($"SQL query : {queryStopwatch.Elapsed.TotalMilliseconds.ToString(CultureInfo.InvariantCulture)} milliseconds.",
                database.ServerName,
                database.DatabaseName);

            return result;
        }

        private static ExecutionResult GetExecutionResult(SqlDataReader reader, Database database)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (reader.IsClosed)
            {
                throw new ArgumentException("Cannot read a closed data reader.", nameof(reader));
            }

            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            var tableSet = new List<Table>();

            do
            {
                var fieldCount = reader.FieldCount;
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

                Logger.Instance.Info($"Rows in table : {table.Rows.Count}", 
                    database.ServerName,
                    database.DatabaseName);
            } while (reader.NextResult());

            reader.Close();

            // If the number of records affected is -1, it means it is a SELECT statement.
            if (reader.RecordsAffected != -1)
            {
                Logger.Instance.Info($"Records affected by query : {reader.RecordsAffected}", 
                    database.ServerName,
                    database.DatabaseName);
            }

            var result = new ExecutionResult(database.ServerName, database.DatabaseName, tableSet);

            return result;
        }
    }
}
