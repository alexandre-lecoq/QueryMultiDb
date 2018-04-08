using CommandLine;

namespace QueryMultiDb
{
    public class CommandLineParameters
    {
        [Option(HelpText = "Indicates output directory for generated file. The default is the current working directory.", Default = null)]
        public string OutputDirectory { get; set; }

        [Option(HelpText = "Indicates the name of the generated file.", Required = true)]
        public string OutputFile { get; set; }

        [Option(HelpText = "Overwrite output file if it already exists.", Default = false)]
        public bool Overwrite { get; set; }
        
        [Option(HelpText = "Indicates the list of databases to query.", Default = null)]
        public string Targets { get; set; }

        [Option(HelpText = "Indicates the list of databases to query is read from standard input.", Default = false)]
        public bool TargetsStandardInput { get; set; }

        [Option(HelpText = "Indicates the file containing the list of databases to query.", Default = null)]
        public string TargetsFile { get; set; }

        [Option(HelpText = "Indicates the query to execute.", Default = null)]
        public string Query { get; set; }

        [Option(HelpText = "Indicates the file containing the SQL query to execute.", Default = null)]
        public string QueryFile { get; set; }

        [Option(HelpText = "Add debug messages to output.", Default = false)]
        public bool Debug { get; set; }

        [Option(HelpText = "Perform queries one at a time.", Default = false)]
        public bool Sequential { get; set; }

        [Option(HelpText = "The time (in seconds) to wait for a connection to open.", Default = 5)]
        public int ConnectionTimeout { get; set; }

        [Option(HelpText = "The time in seconds to wait for the command to execute.", Default = 60)]
        public int CommandTimeout { get; set; }

        [Option(HelpText = "The maximum number of queries running in parallel.", Default = 4)]
        public int Parallelism { get; set; }

        [Option(HelpText = "Include server's IP address.", Default = false)]
        public bool IncludeIP { get; set; }

        [Option(HelpText = "Do not output information messages to console.", Default = true)]
        public bool Quiet { get; set; }

        [Option(HelpText = "Wait for a key press to start.", Default = false)]
        public bool StartKeyPress { get; set; }

        [Option(HelpText = "Wait for a key press to stop.", Default = false)]
        public bool StopKeyPress { get; set; }

        /// XXX : Issue #42
        [Option(HelpText = "Show NULL values explicitly rather than showing empty value.", Default = false)]
        public bool ShowNulls { get; set; }

        [Option(HelpText = "Reports progress on standard error output.", Default = false)]
        public bool Progress { get; set; }

        [Option(HelpText = "Indicates the color of the NULL text in excel files.", Default = "7F7F7F")]
        public string NullsColor { get; set; }

    }
}
