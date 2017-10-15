using System;
using System.Linq;

namespace QueryMultiDb
{
    public static class TableColumnArrayExtensions
    {
        public static bool IsIdentical(this TableColumn[] columns1, TableColumn[] columns2)
        {
            if (columns1 == null)
            {
                throw new ArgumentNullException(nameof(columns1), "Parameter cannot be null.");
            }

            if (columns2 == null)
            {
                throw new ArgumentNullException(nameof(columns2), "Parameter cannot be null.");
            }

            if (columns1.Length != columns2.Length)
            {
                return false;
            }

            for (var i = 0; i < columns1.Length; i++)
            {
                if (!columns1[i].Equals(columns2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsSupersetOf(this TableColumn[] columns1, TableColumn[] columns2)
        {
            if (columns1 == null)
            {
                throw new ArgumentNullException(nameof(columns1), "Parameter cannot be null.");
            }

            if (columns2 == null)
            {
                throw new ArgumentNullException(nameof(columns2), "Parameter cannot be null.");
            }

            if (columns1.Length < columns2.Length)
            {
                return false;
            }

            var columns1List = columns1.ToList();
            var columns2List = columns2.ToList();
            
            for (var i = columns2List.Count - 1; i >= 0 ; i--)
            {
                var tableColumn = columns2List[i];
                columns2List.RemoveAt(i);

                var removed = false;

                for (var j = columns1List.Count - 1; j >= 0; j--)
                {
                    if (!tableColumn.Equals(columns1List[j]))
                    {
                        continue;
                    }

                    columns1List.RemoveAt(j);
                    removed = true;
                    break;
                }

                if (!removed)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
