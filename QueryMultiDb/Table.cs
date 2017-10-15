using System;
using System.Collections.Generic;

namespace QueryMultiDb
{
    /// <summary>
    /// Represents a low memory comsumption table storage.
    /// </summary>
    public struct Table : IEquatable<Table>
    {
        public TableColumn[] Columns { get; }

        public ICollection<TableRow> Rows { get; }

        public Table(TableColumn[] columns, ICollection<TableRow> rows)
        {
            if (columns == null)
            {
                throw new ArgumentNullException(nameof(columns), "Parameter cannot be null.");
            }

            if (rows == null)
            {
                throw new ArgumentNullException(nameof(rows), "Parameter cannot be null.");
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
    }
}
