using System.Collections.Generic;
using System.IO;

namespace QueryMultiDb.Exporter
{
    public interface IExporter
    {
        string Name { get; }

        void Generate(ICollection<Table> inputTables);

        void Generate(ICollection<Table> inputTables, Stream outputStream);
    }
}
