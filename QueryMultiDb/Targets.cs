using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryMultiDb
{
    public class TargetSet
    {
        public IEnumerable<Database> Databases { get; }

        public string[] ExtraValueTitles { get; }
        
        public TargetSet(IEnumerable<Database> databases, string[] extraValueTitles)
        {
            if (databases == null)
            {
                throw new ArgumentNullException(nameof(databases));
            }

            if (extraValueTitles == null)
            {
                throw new ArgumentNullException(nameof(extraValueTitles));
            }

            Databases = databases;
            ExtraValueTitles = extraValueTitles;
        }

        public override string ToString()
        {
            return $"Databases = \"{Databases.Count()}\"";
        }
    }
}
