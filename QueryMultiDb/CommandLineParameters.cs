using CommandLineParser.Arguments;
using QueryMultiDb.Exporter;
using System.IO;

namespace QueryMultiDb
{
    public class CommandLineParameters
    {
        [DirectoryArgument(
            "outputdirectory",
            DirectoryMustExist = true,
            Description = "Indicates output directory for generated file.",
            FullDescription = "This is the output directory where the generated excel file will be written. The default is the current working directory.",
            Example = @"c:\temp\")]
        public DirectoryInfo OutputDirectory { get; set; }

        [ValueArgument(
            typeof(string),
            "outputfile",
            Optional = false,
            ValueOptional = false,
            Description = "Indicates the name of the generated file.",
            FullDescription = "This is the name of the generated excel file containing the resulting data.",
            Example = "file.xlsx")]
        public string OutputFile { get; set; }

        [SwitchArgument(
            "overwrite",
            false,
            Description = "Overwrite output file if it already exists.",
            FullDescription = "If this switch is turned on, the file will be overwritten if it already exists. Otherwise, the execution will stop and an error message will be displayed if the file already exists.")]
        public bool Overwrite { get; set; }

        [ValueArgument(
            typeof(string),
            "targets",
            DefaultValue = null,
            Description = "Indicates the list of databases to query.",
            FullDescription = "This is a JSON-formatted list of databases to query.",
            Example = @"{ ""DatabaseList"": [ { ""ServerName"": ""localhost"", ""DatabaseName"": ""FUNNY_DB"" } ] }")]
        public string Targets { get; set; }

        [SwitchArgument(
            "targetsstandardinput",
            false,
            Description = "Indicates the list of databases to query is read from standard input.",
            FullDescription = "If this switch is turned on, targets are read from the standard input.")]
        public bool TargetsStandardInput { get; set; }

        [FileArgument(
            "targetsfile",
            FileMustExist = true,
            Description = "Indicates the file containing the list of databases to query.",
            FullDescription = "If this parameter is set to an existing file path, targets will be read from that file.",
            Example = @"c:\temp\dbs.targets")]
        public FileInfo TargetsFile { get; set; }

        [ValueArgument(
            typeof(string),
            "query",
            DefaultValue = null,
            Description = "Indicates the query to execute.",
            FullDescription = "This is the SQL query to execute on each database.",
            Example = "SELECT TOP 5 * FROM Table")]
        public string Query { get; set; }

        [FileArgument(
            "queryfile",
            FileMustExist = true,
            Description = "Indicates the file containing the query to execute.",
            FullDescription = "If this parameter is set to an existing file path, the query in this file will be execute on each database.",
            Example = @"c:\temp\query.sql")]
        public FileInfo QueryFile { get; set; }

        [SwitchArgument(
            "sequential",
            false,
            Description = "Perform queries one at a time.",
            FullDescription = "If this switch is turned on, the query won't be executed on several databases in the same time.")]
        public bool Sequential { get; set; }

        [ValueArgument(
            typeof(int),
            "connectiontimeout",
            DefaultValue = 5,
            Description = "The time to wait for a connection to open.",
            FullDescription = "The time (in seconds) to wait for a connection to open. The default value is 5 seconds.",
            Example = "15")]
        public int ConnectionTimeout { get; set; }

        [ValueArgument(
            typeof(int),
            "commandtimeout",
            DefaultValue = 60,
            Description = "The time to wait for the command to execute.",
            FullDescription = "The time (in seconds) to wait for the command to execute. The default value is 60 seconds.",
            Example = "30")]
        public int CommandTimeout { get; set; }

        [ValueArgument(
            typeof(int),
            "parallelism",
            DefaultValue = 4,
            Description = "The maximum number of queries running in parallel.",
            FullDescription = "When parallelism is used one blocked or long running query will not prevent new queries to start running. If the value is 1, queries will run sequentially. The higher the value the more databases will be queried at the same time. The list of database targets is shuffled such that queries running in parallel are not necessarily those close in the target list.",
            Example = "8")]
        public int Parallelism { get; set; }

        [SwitchArgument(
            "startkeypress",
            false,
            Description = "Wait for a key press to start.",
            FullDescription = "Wait for a key press to start. This is useful when running the program outside of console.")]
        public bool StartKeyPress { get; set; }

        [SwitchArgument(
            "stopkeypress",
            false,
            Description = "Wait for a key press to stop.",
            FullDescription = "Wait for a key press to stop. This is useful when running the program outside of console.")]
        public bool StopKeyPress { get; set; }

        [SwitchArgument(
            "progress",
            false,
            Description = "Reports progress on standard error output.",
            FullDescription = "Displays the progress percentage and item count on the standard error output of the console.")]
        public bool Progress { get; set; }

        [RegexValueArgument(
            "nullscolor",
            "^(?:[0-9a-fA-F]{6})$",
            DefaultValue = "7F7F7F",
            Description = "Indicates the color of the NULL text in excel files.",
            FullDescription = "This is the color the NULL values will be displayed in excel. The default is gray.",
            Example = "EEAD0E",
            SampleValue = "FF1493")]
        public string NullsColor { get; set; }

        [EnumeratedValueArgument(
            typeof(bool),
            "shownulls",
            AllowedValues = "true;false",
            DefaultValue = true,
            ValueOptional = true,
            Description = "Show NULL values explicitly rather than showing empty value.",
            FullDescription = "If this switch is turned on, NULL values will appear as such in excel file. Otherwise the cells will be empty.",
            Example = "false")]
        public bool ShowNulls { get; set; }

        [EnumeratedValueArgument(
            typeof(bool),
            "showipaddress",
            AllowedValues = "true;false",
            DefaultValue = true,
            ValueOptional = true,
            Description = "Show server's IP address.",
            FullDescription = "If this switch is turned on, the excel file will contain a column displaying server's IP address.",
            Example = "false")]
        public bool ShowIpAddress { get; set; }

        [EnumeratedValueArgument(
            typeof(bool),
            "showservername",
            AllowedValues = "true;false",
            DefaultValue = true,
            ValueOptional = true,
            Description = "Show server's name.",
            FullDescription = "If this switch is turned on, the excel file will contain a column displaying server's name.",
            Example = "false")]
        public bool ShowServerName { get; set; }

        [EnumeratedValueArgument(
            typeof(bool),
            "showdatabasename",
            AllowedValues = "true;false",
            DefaultValue = true,
            ValueOptional = true,
            Description = "Show database's name.",
            FullDescription = "If this switch is turned on, the excel file will contain a column displaying database's name.",
            Example = "false")]
        public bool ShowDatabaseName { get; set; }

        [EnumeratedValueArgument(
            typeof(bool),
            "showextracolumns",
            AllowedValues = "true;false",
            DefaultValue = true,
            ValueOptional = true,
            Description = "Show targets' extra columns.",
            FullDescription = "If this switch is turned on, the excel file will contain columns displaying extra information.",
            Example = "false")]
        public bool ShowExtraColumns { get; set; }

        [EnumeratedValueArgument(
            typeof(bool),
            "showlogsheet",
            AllowedValues = "true;false",
            DefaultValue = true,
            ValueOptional = true,
            Description = "Show log sheet in excel file.",
            FullDescription = "If this switch is turned on, the excel file will contain a sheet with execution logs.",
            Example = "false")]
        public bool ShowLogSheet { get; set; }

        [EnumeratedValueArgument(
            typeof(bool),
            "showparametersheet",
            AllowedValues = "true;false",
            DefaultValue = true,
            ValueOptional = true,
            Description = "Show parameter sheet in excel file.",
            FullDescription = "If this switch is turned on, the excel file will contain a sheet with execution parameters.",
            Example = "false")]
        public bool ShowParameterSheet { get; set; }

        [EnumeratedValueArgument(
            typeof(bool),
            "showinformationmessages",
            AllowedValues = "true;false",
            DefaultValue = true,
            ValueOptional = true,
            Description = "Show information messages sheet in excel file.",
            FullDescription = "If this switch is turned on, the excel file will contain a sheet with SQL information messages.",
            Example = "false")]
        public bool ShowInformationMessages { get; set; }

        [ValueArgument(
            typeof(string),
            "sheetlabels",
            DefaultValue = null,
            Description = "Defines the sheets' labels.",
            FullDescription = "This is a comma-separated list of sheet name to be used for query results sheets in excel file.",
            Example = "Sheet 1, Sheet 2, Sheet 3")]
        public string SheetLabels { get; set; }

        [SwitchArgument(
            "discardresults",
            false,
            Description = "Discard query results and display counts instead.",
            FullDescription = "If the switch is turned on, results will be discarded. A field and row count will be calculated instead. It will save memory and file generation time in this software.")]
        public bool DiscardResults { get; set; }

        [ValueArgument(
            typeof(string),
            "applicationname",
            DefaultValue = null,
            Description = "Defines the application name for the SQL server connection.",
            FullDescription = "You can use this to specify the name of the application running this tool. The name will be visible in SQL server's connection information.",
            Example = "MyApplication")]
        public string ApplicationName { get; set; }

        [ValueArgument(
            typeof(ExporterType),
            "exporter",
            DefaultValue = ExporterType.Excel,
            ValueOptional = false,
            Description = "Selects the output file format.",
            FullDescription = "Indicates the output file format. The file format is excel.",
            Example = "csv")]
        public ExporterType Exporter { get; set; }

        [ValueArgument(
            typeof(string),
            "csvdelimiter",
            DefaultValue = ";",
            Description = "Defines the CSV delimiter used to separate fields.",
            FullDescription = "You can use this to define the CSV delimiter used to separate fields. Common delimiter are comma, semicolon and TAB. The default delimiter is semicolon.",
            Example = ";")]
        public string CsvDelimiter { get; set; }

        [ValueArgument(
            typeof(int),
            "base10threshold",
            DefaultValue = 4,
            Description = "The inclusive maximum number of bytes for which to use decimal representation.",
            FullDescription = "Use this to define the inclusive maximum number of bytes for which to use decimal representation of binary data.",
            Example = "8")]
        public int Base10Threshold { get; set; }

        [ValueArgument(
            typeof(int),
            "base16threshold",
            DefaultValue = 64,
            Description = "The inclusive maximum number of bytes for which to use hexadecimal representation.",
            FullDescription = "Use this to define the inclusive maximum number of bytes for which to use hexadecimal representation of binary data.",
            Example = "128")]
        public int Base16Threshold { get; set; }

        [ValueArgument(
            typeof(int),
            "base64threshold",
            DefaultValue = 262144,
            Description = "The inclusive maximum number of bytes for which to use base 64 representation.",
            FullDescription = "Use this to define the inclusive maximum number of bytes for which to use base 64 representation of binary data.",
            Example = "524288")]
        public int Base64Threshold { get; set; }
    }
}
