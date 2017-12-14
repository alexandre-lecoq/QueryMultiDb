using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using CommandLine;

namespace QueryMultiDb
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            SetCulture();

            var parserResult = Parser.Default.ParseArguments<CommandLineParameters>(args);

            if (parserResult.Tag != ParserResultType.Parsed)
            {
                return -1;
            }

            var parsedResult = (Parsed<CommandLineParameters>)parserResult;
            // This must be set very early to be usable at any time.
            Parameters.Instance = new Parameters(parsedResult.Value);

            try
            {
                if (Parameters.Instance.StartKeyPress)
                {
                    Console.WriteLine("Press a key to start...");
                    Console.ReadKey();
                }

                if (Parameters.Instance.Debug)
                {
                    Logger.Instance.Info("Debug mode is active.");
                }

                DoIt();

                return 0;
            }
            catch (Exception exp)
            {
                Logger.Instance.Error("Fatal error.", exp);

                return -2;
            }
            finally
            {
                if (Parameters.Instance.StopKeyPress)
                {
                    Console.WriteLine("Press a key to stop...");
                    Console.ReadKey();
                }
            }
        }

        private static void SetCulture()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        private static void DoIt()
        {
            var queryStopwatch = new Stopwatch();
            queryStopwatch.Start();
            var result = DataReader.GetQueryResults();
            queryStopwatch.Stop();
            Logger.Instance.Info($"Query results : {queryStopwatch.Elapsed.TotalMilliseconds.ToString(CultureInfo.InvariantCulture)} milliseconds.");

            var mergeStopwatch = new Stopwatch();
            mergeStopwatch.Start();
            var mergedResults = DataMerger.MergeResults(result);
            mergeStopwatch.Stop();
            Logger.Instance.Info($"Merged results : {mergeStopwatch.Elapsed.TotalMilliseconds.ToString(CultureInfo.InvariantCulture)} milliseconds.");

            var excelGenerationStopwatch = new Stopwatch();
            excelGenerationStopwatch.Start();
            ExcelExporter.Generate(mergedResults);
            excelGenerationStopwatch.Stop();
            Logger.Instance.Info($"Excel generation : {excelGenerationStopwatch.Elapsed.TotalMilliseconds.ToString(CultureInfo.InvariantCulture)} milliseconds.");
        }
    }
}
