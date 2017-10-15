using CommandLine;

namespace QueryMultiDb
{
    public class CommandLineParameters
    {
        [Option(HelpText = "Indicates output directory for generated file. The default is the current working directory.", Default = null)]
        public string OutputDirectory { get; set; }

        [Option(HelpText = "Indicates the name of the generated file.", Required = true)]
        public string OutputFile { get; set; }

        [Option(HelpText = "Overwrite output file if it already exists. The default is false.", Default = false)]
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

        [Option(HelpText = "Add debug messages to output. The default is false.", Default = false)]
        public bool Debug { get; set; }

        [Option(HelpText = "Perform queries one at a time. The default is false.", Default = false)]
        public bool Sequential { get; set; }

        [Option(HelpText = "The time (in seconds) to wait for a connection to open. The default value is 5 seconds.", Default = 5)]
        public int ConnectionTimeout { get; set; }

        [Option(HelpText = "The time in seconds to wait for the command to execute. The default is 60 seconds.", Default = 60)]
        public int CommandTimeout { get; set; }

        [Option(HelpText = "The maximum number of queries running in parallel. The default is 4.", Default = 4)]
        public int Parallelism { get; set; }

        [Option(HelpText = "Include server's IP address. The default is false.", Default = false)]
        public bool IncludeIP { get; set; }

        [Option(HelpText = "Do not output information messages to console. The default is true.", Default = true)]
        public bool Quiet { get; set; }
    }
}
