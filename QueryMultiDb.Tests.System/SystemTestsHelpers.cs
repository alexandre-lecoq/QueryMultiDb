using DocumentFormat.OpenXml.Packaging;
using QueryMultiDb.Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace QueryMultiDb.Tests.System
{
    public static class SystemTestsHelpers
    {
        private const string RelativeTestResultsDirectory = "../../../TestResults/";

        public static SystemExecutionOutput RunQueryMultiDbExecutionFromData(string query, string targets, QueryMultiDbArgumentStringBuilder argumentStringBuilder)
        {
            if (argumentStringBuilder == null)
                throw new ArgumentNullException(nameof(argumentStringBuilder));

            argumentStringBuilder.Query = query;
            argumentStringBuilder.Targets = targets;
            var systemExecutionOutput = RunQueryMultiDbExecution(argumentStringBuilder);

            return systemExecutionOutput;
        }

        public static SystemExecutionOutput RunQueryMultiDbExecutionFromResource(string resourceNameSql, string resourceNameJson, bool formatResource, QueryMultiDbArgumentStringBuilder argumentStringBuilder)
        {
            if (argumentStringBuilder == null)
                throw new ArgumentNullException(nameof(argumentStringBuilder));

            var temporaryDirectory = GetTemporaryDirectory();
            CopyResourceToDirectory(resourceNameSql, temporaryDirectory, formatResource);
            CopyResourceToDirectory(resourceNameJson, temporaryDirectory, formatResource);
            argumentStringBuilder.QueryFile = Path.Combine(temporaryDirectory, resourceNameSql);
            argumentStringBuilder.TargetsFile = Path.Combine(temporaryDirectory, resourceNameJson);
            var systemExecutionOutput = RunQueryMultiDbExecution(argumentStringBuilder);
            DeleteDirectory(temporaryDirectory);

            return systemExecutionOutput;
        }

        private static SystemExecutionOutput RunQueryMultiDbExecution(QueryMultiDbArgumentStringBuilder argumentStringBuilder)
        {
            var temporaryDirectory = GetTemporaryDirectory();
            var outputFilename = Guid.NewGuid().ToString();
            argumentStringBuilder.OutputFile = Path.Combine(temporaryDirectory, outputFilename);

            var arguments = argumentStringBuilder.ToString();

            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                FileName = ExecutableResolver.GetQueryMultiDbExecutablePath(),
                Arguments = arguments
            };

            SystemExecutionOutput systemExecutionOutput;

            using (var process = Process.Start(startInfo))
            {
                if (process == null)
                {
                    throw new Exception();
                }

                string standardOutput;

                using (var standardOutputReader = process.StandardOutput)
                {
                    standardOutput = standardOutputReader.ReadToEnd();
                }

                string standardError;

                using (var standardErrorReader = process.StandardError)
                {
                    standardError = standardErrorReader.ReadToEnd();
                }

                process.WaitForExit();

                var exitCode = process.ExitCode;

                byte[] fileContent = null;

                try
                {
                    var path = Path.Combine(temporaryDirectory, outputFilename);
                    fileContent = File.ReadAllBytes(path);
                    SafeCreateDirectory(RelativeTestResultsDirectory);
                    var extension = argumentStringBuilder.Exporter == "csv" ? ".zip" : ".xlsx";
                    File.Move(path, RelativeTestResultsDirectory + outputFilename + extension);
                }
                catch
                {
                    // Ignored because fileContent being null is fine in this case. It will be asserted later.
                    throw;
                }

                systemExecutionOutput = new SystemExecutionOutput(exitCode, standardOutput, standardError, fileContent);
            }

            DeleteDirectory(temporaryDirectory);

            return systemExecutionOutput;
        }

        private static void SafeCreateDirectory(string relativeTestResultsDirectory)
        {
            if (Directory.Exists(relativeTestResultsDirectory))
            {
                return;
            }

            try
            {
                Directory.CreateDirectory(relativeTestResultsDirectory);
            }
            catch
            {
                // Probably a race condition. Let's ignore it.
            }
        }

        public static void AssertStandardSuccessConditions(SystemExecutionOutput systemExecutionOutput)
        {
            if (systemExecutionOutput == null)
                throw new ArgumentNullException(nameof(systemExecutionOutput));

            Assert.Equal(0, systemExecutionOutput.ExitCode);

            Assert.Matches("Query results :", systemExecutionOutput.StandardOutput);
            Assert.Matches("Merged results with", systemExecutionOutput.StandardOutput);
            Assert.Matches(" generation :", systemExecutionOutput.StandardOutput);
            Assert.DoesNotMatch("FATAL", systemExecutionOutput.StandardOutput);

            Assert.NotNull(systemExecutionOutput.OutputFileContent);
            Assert.True(systemExecutionOutput.OutputFileContent.Length > 0);
            Assert.True(systemExecutionOutput.OutputFileContent.Length >= 2);
            // The file is actually a ZIP file. ZIP files start with magic header "PK" {80;75}.
            Assert.Equal(80, systemExecutionOutput.OutputFileContent[0]);
            Assert.Equal(75, systemExecutionOutput.OutputFileContent[1]);
        }

        public static int AssertStandardExcelSuccessConditions(SystemExecutionOutput systemExecutionOutput)
        {
            Assert.Matches("Excel generation :", systemExecutionOutput.StandardOutput);
            var sheetCount = ReadSheetCountFromSpreadsheet(systemExecutionOutput.OutputFileContent);
            Assert.True(sheetCount > 0);

            return sheetCount;
        }

        private static int ReadSheetCountFromSpreadsheet(byte[] spreadsheetBinaryContent)
        {
            var stream = new MemoryStream(spreadsheetBinaryContent, false);

            int sheetCount;

            using (var spreadsheetDocument = SpreadsheetDocument.Open(stream, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var workbookSheets = workbookPart.Workbook.Sheets;
                sheetCount = workbookSheets.Count();
            }

            return sheetCount;
        }

        private static string GetTemporaryDirectory()
        {
            var tempPath = Path.GetTempPath();
            var fileName = Path.GetRandomFileName();
            var tempDirectory = Path.Combine(tempPath, fileName);
            Directory.CreateDirectory(tempDirectory);

            return tempDirectory;
        }

        private static void CopyResourceToDirectory(string resourceName, string directory, bool format = false)
        {
            var resourceText = GetResource(resourceName, format);
            var path = Path.Combine(directory, resourceName);

            using (var fileStream = File.Create(path))
            using (var writer = new StreamWriter(fileStream))
            {
                writer.Write(resourceText);
            }
        }

        public static string GetResource(string resourceName, bool format = false)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new Exception($"Resource '{resourceName}' likely does not exist.");
                }

                using (var streamReader = new StreamReader(stream))
                {
                    var text = streamReader.ReadToEnd();

                    var resourceText = format ? DatabaseFixture.FormatTargets(text) : text;

                    return resourceText;
                }
            }
        }

        private static void DeleteDirectory(string directory)
        {
            var dir = new DirectoryInfo(directory);
            dir.Delete(true);
        }
    }
}
