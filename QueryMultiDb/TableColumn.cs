using System;

namespace QueryMultiDb
{
    public struct TableColumn : IEquatable<TableColumn>
    {
        public string ColumnName { get; }

        public Type DataType { get; }

        public TableColumn(string columnName, Type dataType)
        {
            ColumnName = columnName;
            DataType = dataType;
        }

        public static bool operator ==(TableColumn left, TableColumn right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TableColumn left, TableColumn right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TableColumn))
            {
                return false;
            }

            var tableColumn = (TableColumn) obj;

            return Equals(tableColumn);
        }

        public bool Equals(TableColumn other)
        {
            return ColumnName == other.ColumnName && DataType == other.DataType;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = ((ColumnName != null ? ColumnName.GetHashCode() : 0) * 397) ^
                           (DataType != null ? DataType.GetHashCode() : 0);
                return hash;
            }
        }

        public override string ToString()
        {
            return $"{ColumnName} {DataType}";
        }
    }
}