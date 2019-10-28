namespace QueryMultiDb.Common
{
    using System.Text;

    public class QueryMultiDbArgumentStringBuilder
    {
        public bool? Overwrite { get; set; }

        public bool? Progress { get; set; }

        public string QueryFile { get; set; }

        public string TargetsFile { get; set; }

        public string OutputFile { get; set; }

        public bool? ShowNulls { get; set; }

        public string NullsColor { get; set; }

        public bool? DiscardResults { get; set; }

        public int? CommandTimeout { get; set; }

        public int? Parallelism { get; set; }

        public string SheetLabels { get; set; }

        public string ApplicationName { get; set; }

        public bool? ShowIpAddress { get; set; }

        public bool? ShowServerName { get; set; }

        public bool? ShowDatabaseName { get; set; }

        public bool? ShowExtraColumns { get; set; }

        public bool? ShowLogSheet { get; set; }

        public bool? ShowParameterSheet { get; set; }

        public bool? ShowInformationMessages { get; set; }

        public string Exporter { get; set; }

        public string CsvDelimiter { get; set; }

        public int? Base10Threshold { get; set; }

        public int? Base16Threshold { get; set; }

        public int? Base64Threshold { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (Overwrite.HasValue)
            {
                if (Overwrite == true)
                {
                    sb.Append(" --overwrite");
                }
            }

            if (Progress.HasValue)
            {
                if (Progress == true)
                {
                    sb.Append(" --progress");
                }
            }

            if (!string.IsNullOrWhiteSpace(QueryFile))
            {
                sb.Append($@" --queryfile ""{QueryFile}""");
            }

            if (!string.IsNullOrWhiteSpace(TargetsFile))
            {
                sb.Append($@" --targetsfile ""{TargetsFile}""");
            }

            if (!string.IsNullOrWhiteSpace(OutputFile))
            {
                sb.Append($@" --outputfile ""{OutputFile}""");
            }

            if (ShowNulls.HasValue)
            {
                sb.Append($@" --shownulls {ShowNulls.ToString().ToLowerInvariant()}");
            }

            if (!string.IsNullOrWhiteSpace(NullsColor))
            {
                sb.Append($@" --nullscolor ""{NullsColor}""");
            }

            if (DiscardResults.HasValue)
            {
                if (DiscardResults == true)
                {
                    sb.Append(@" --discardresults");
                }
            }

            if (CommandTimeout.HasValue)
            {
                sb.Append($@" --commandtimeout {CommandTimeout}");
            }

            if (Parallelism.HasValue)
            {
                sb.Append($@" --parallelism {Parallelism}");
            }
            
            if (!string.IsNullOrWhiteSpace(SheetLabels))
            {
                sb.Append($@" --sheetlabels ""{SheetLabels}""");
            }
            
            if (!string.IsNullOrWhiteSpace(ApplicationName))
            {
                sb.Append($@" --applicationname ""{ApplicationName}""");
            }

            if (ShowIpAddress.HasValue)
            {
                sb.Append($@" --showipaddress {ShowIpAddress.ToString().ToLowerInvariant()}");
            }

            if (ShowServerName.HasValue)
            {
                sb.Append($@" --showservername {ShowServerName.ToString().ToLowerInvariant()}");
            }

            if (ShowDatabaseName.HasValue)
            {
                sb.Append($@" --showdatabasename {ShowDatabaseName.ToString().ToLowerInvariant()}");
            }

            if (ShowExtraColumns.HasValue)
            {
                sb.Append($@" --showextracolumns {ShowExtraColumns.ToString().ToLowerInvariant()}");
            }

            if (ShowLogSheet.HasValue)
            {
                sb.Append($@" --showlogsheet {ShowLogSheet.ToString().ToLowerInvariant()}");
            }

            if (ShowParameterSheet.HasValue)
            {
                sb.Append($@" --showparametersheet {ShowParameterSheet.ToString().ToLowerInvariant()}");
            }

            if (ShowInformationMessages.HasValue)
            {
                sb.Append($@" --showinformationmessages {ShowInformationMessages.ToString().ToLowerInvariant()}");
            }
            
            if (Exporter != null)
            {
                sb.Append($@" --exporter {Exporter}");
            }

            if (CsvDelimiter != null)
            {
                sb.Append($@" --csvdelimiter {CsvDelimiter}");
            }

            if (Base10Threshold.HasValue)
            {
                sb.Append($@" --base10threshold {Base10Threshold}");
            }

            if (Base16Threshold.HasValue)
            {
                sb.Append($@" --base16threshold {Base16Threshold}");
            }

            if (Base64Threshold.HasValue)
            {
                sb.Append($@" --base64threshold {Base64Threshold}");
            }

            return sb.ToString();
        }
    }
}
