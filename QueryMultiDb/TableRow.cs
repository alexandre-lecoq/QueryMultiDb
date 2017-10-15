using System;

namespace QueryMultiDb
{
    public struct TableRow : IEquatable<TableRow>
    {
        public object[] ItemArray { get; }

        public TableRow(object[] itemArray)
        {
            if (itemArray == null)
            {
                throw new ArgumentNullException(nameof(itemArray), "Parameter cannot be null.");
            }

            ItemArray = itemArray;
        }

        public override string ToString()
        {
            return $"Row value count = {ItemArray.Length}";
        }

        public bool Equals(TableRow other)
        {
            return ItemArray.Equals(other.ItemArray);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TableRow && Equals((TableRow) obj);
        }

        public override int GetHashCode()
        {
            return ItemArray.GetHashCode();
        }

        public static bool operator ==(TableRow left, TableRow right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TableRow left, TableRow right)
        {
            return !left.Equals(right);
        }
    }
}
