using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryMultiDb
{
    public class TargetSet
    {
        public IEnumerable<Database> Databases { get; }

        public bool[] EmptyExtraValues { get; }

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

            EmptyExtraValues = new[] {true, true, true, true, true, true};

            foreach (var database in Databases)
            {
                if (!string.IsNullOrWhiteSpace(database.ExtraValue1))
                {
                    EmptyExtraValues[0] = false;
                }

                if (!string.IsNullOrWhiteSpace(database.ExtraValue2))
                {
                    EmptyExtraValues[1] = false;
                }

                if (!string.IsNullOrWhiteSpace(database.ExtraValue3))
                {
                    EmptyExtraValues[2] = false;
                }

                if (!string.IsNullOrWhiteSpace(database.ExtraValue4))
                {
                    EmptyExtraValues[3] = false;
                }

                if (!string.IsNullOrWhiteSpace(database.ExtraValue5))
                {
                    EmptyExtraValues[4] = false;
                }

                if (!string.IsNullOrWhiteSpace(database.ExtraValue6))
                {
                    EmptyExtraValues[5] = false;
                }
            }

            ExtraValueTitles = extraValueTitles;
        }

        public override string ToString()
        {
            return $"Databases = \"{Databases.Count()}\"";
        }
    }
}
