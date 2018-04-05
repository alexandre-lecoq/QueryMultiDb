using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryMultiDb
{
    /// <summary>
    /// Represents a low memory comsumption table storage.
    /// </summary>
    /// <remarks>
    /// We use are own classes because there is a *huge* memory overhead associated to <see cref="System.Data.DataTable"/> instances.
    /// <see cref="System.Data.DataTable"/> instances can use three to seven times more memory than our simple <see cref="Table"/> instance.
    /// This is important SQL queries can get several gigabytes of data from the database, making the overhead hard to bear.
    /// </remarks>
    public struct Table : IEquatable<Table>
    {
        public const string InformationMessagesId = "__Information_Messages__";
        public const string LogsId = "__Logs__";
        public const string CommandLineParametersId = "__Command_Line_Parameters__";

        public TableColumn[] Columns { get; }

        public ICollection<TableRow> Rows { get; }

        public string Id { get; }
        
        public Table(TableColumn[] columns, ICollection<TableRow> rows, string id = null)
        {
            if (columns == null)
            {
                throw new ArgumentNullException(nameof(columns), "Parameter cannot be null.");
            }

            if (rows == null)
            {
                throw new ArgumentNullException(nameof(rows), "Parameter cannot be null.");
            }

            if (id != null)
            {
                Id = id;
            }
            else
            {
                var columnSetHash = columns.Select(tableColumn => tableColumn.GetHashCode())
                    .Aggregate(0, (current, columnHash) => current ^ columnHash);
                Id = columnSetHash.ToString();
            }

            Columns = columns;
            Rows = rows;
        }

        public override string ToString()
        {
            return $"Rows = {Rows.Count}; Columns = {Columns.Length}";
        }

        public bool Equals(Table other)
        {
            return Columns.Equals(other.Columns) && Rows.Equals(other.Rows);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Table && Equals((Table) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Columns.GetHashCode() * 397) ^ Rows.GetHashCode();
            }
        }

        public static bool operator ==(Table left, Table right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Table left, Table right)
        {
            return !left.Equals(right);
        }

        public bool HasIdenticalColumns(Table otherTable)
        {
            var thisColumns = Columns;
            var otherColumns = otherTable.Columns;

            if (thisColumns.Length != otherColumns.Length)
            {
                return false;
            }

            for (var i = 0; i < thisColumns.Length; i++)
            {
                var thisColumn = thisColumns[i];
                var otherColumn = otherColumns[i];

                if (thisColumn != otherColumn)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
