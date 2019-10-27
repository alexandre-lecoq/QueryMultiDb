using System;

namespace QueryMultiDb.Exporter
{
    public static class ExporterFactory
    {
        public static IExporter GetExporter(ExporterType type)
        {
            switch (type)
            {
                case ExporterType.Csv:
                    return new CsvExporter();
                case ExporterType.Excel:
                    return new ExcelExporter();
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
