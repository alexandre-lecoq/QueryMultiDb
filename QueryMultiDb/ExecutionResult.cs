﻿using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace QueryMultiDb
{
    public class ExecutionResult
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Database Database { get; }

        public IList<Table> TableSet { get; }

        public ExecutionResult(Database database, IList<Table> tableSet)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }
            
            if (tableSet == null)
            {
                throw new ArgumentNullException(nameof(tableSet));
            }

            Database = database;
            TableSet = tableSet;
        }

        public override string ToString()
        {
            var tableCount = TableSet.Count;
            var totalRowCount = TableSet.Sum(table => table.Rows.Count);

            return $"{Database} ; Total row count = {totalRowCount} ; Table count = {tableCount}";
        }

        public bool HasIdenticalTableAndColumns(ExecutionResult other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (TableSet == null && other.TableSet == null)
            {
                return true;
            }

            if (TableSet == null || other.TableSet == null)
            {
                LogTableComparisonWarning(this, other, "One of them does not contain any tables");
                return false;
            }
 
            if (TableSet.Count != other.TableSet.Count)
            {
                LogTableComparisonWarning(this, other, $"Results have different number of tables : {TableSet.Count} vs {other.TableSet.Count}");
                return false;
            }

            for (var i = 0; i < TableSet.Count; i++)
            {
                var thisTable = TableSet[i];
                var otherTable = other.TableSet[i];

                var isIdentical = thisTable.HasIdenticalColumns(otherTable);

                if (!isIdentical)
                {
                    LogTableComparisonWarning(this, other, $"Tables #{i} have different column set");
                    return false;
                }
            }

            return true;
        }

        private static void LogTableComparisonWarning(ExecutionResult left, ExecutionResult right, string specificMessage)
        {
            var message = $"Tables are not identical. In {left.Database.ServerName} {left.Database.DatabaseName} and {right.Database.ServerName} {right.Database.DatabaseName}. {specificMessage}.";
            Logger.Warn(message);
        }
    }
}