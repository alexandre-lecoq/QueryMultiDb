using System.Text;
using NotImplementedException = System.NotImplementedException;

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

        public string ToVerboseString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"ExitCode : {ExitCode}");
            sb.AppendLine($"StandardOutput : {StandardOutput}");
            sb.AppendLine($"StandardError : {StandardError}");
            sb.AppendLine($"OutputFileContent.Length : {OutputFileContent.Length}");

            return sb.ToString();
        }
    }
}
