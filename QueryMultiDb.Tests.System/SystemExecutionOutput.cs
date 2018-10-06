namespace QueryMultiDb.Tests.System
{
    public class SystemExecutionOutput
    {
        public int ExitCode { get; }

        public string StandardOutput { get; }

        public string StandardError { get; }

        public byte[] OutputFileContent { get; }

        public SystemExecutionOutput(int exitCode, string standardOutput, string standardError, byte[] outputFileContent)
        {
            ExitCode = exitCode;
            StandardOutput = standardOutput;
            StandardError = standardError;
            OutputFileContent = outputFileContent;
        }

        public override string ToString()
        {
            return $"ExitCode = {ExitCode} ; OutputFileContent.Length = {OutputFileContent?.Length}";
        }
    }
}
