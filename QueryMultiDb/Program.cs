using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using CommandLine;
using NLog;
using NLog.Config;
using NLog.Targets.Wrappers;

namespace QueryMultiDb
{
    internal class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int Main(string[] args)
        {
            ExtendLogConfigurationWithTableTarget();
            LogManager.ConfigurationReloaded += LogManager_ConfigurationReloaded;
            LogManager.ConfigurationChanged += LogManager_ConfigurationChanged;

            Logger.Info("Initializing QueryMultiDb");

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
                    Logger.Info("Debug mode is active.");
                    Logger.Fatal("Fatal log test");
                    Logger.Error("Error log test");
                    Logger.Warn("Warn log test");
                    Logger.Info("Info log test");
                    Logger.Debug("Debug log test");
                    Logger.Trace("Trace log test");
                }

                DoIt();

                return 0;
            }
            catch (Exception exp)
            {
                Logger.Error(exp, "Fatal error.");

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

        private static void LogManager_ConfigurationChanged(object sender, LoggingConfigurationChangedEventArgs e)
        {
            ExtendLogConfigurationWithTableTarget();
            Logger.Info("Logger configuration changed.");
        }

        private static void LogManager_ConfigurationReloaded(object sender, LoggingConfigurationReloadedEventArgs e)
        {
            ExtendLogConfigurationWithTableTarget();
            Logger.Info("Logger configuration reloaded.");
        }

        /// <summary>
        /// Set up logger table target.
        /// </summary>
        /// <remarks>
        /// This is setup by code because the target is also used by code to recover the content of the logged messages.
        /// The target is recovered by code using :
        /// <code>LogManager.Configuration.FindTargetByName&lt;AutoFlushTargetWrapper&gt;("flushedTableTarget");</code>
        /// </remarks>
        private static void ExtendLogConfigurationWithTableTarget()
        {
            var tableTarget = new TableTarget {Name = "tableTarget"};
            var autoFlushTargetWrapper = new AutoFlushTargetWrapper
            {
                WrappedTarget = tableTarget,
                Name = "flushedTableTarget"
            };

            LogManager.Configuration.AddTarget("flushedTableTarget", autoFlushTargetWrapper);
            var loggingRule = new LoggingRule("*", LogLevel.Trace, autoFlushTargetWrapper);
            LogManager.Configuration.LoggingRules.Add(loggingRule);
            LogManager.ReconfigExistingLoggers();

            var targetByName = LogManager.Configuration.FindTargetByName<AutoFlushTargetWrapper>("flushedTableTarget");

            if (targetByName == null)
            {
                throw new NLogRuntimeException("Could not retrieve target 'flushedTableTarget' by name.");
            }

            if (targetByName.WrappedTarget == null)
            {
                throw new NLogRuntimeException("Target wrapper 'flushedTableTarget' does not contain a wrapped target.");
            }

            var retrievedTableTarget = targetByName.WrappedTarget as TableTarget;

            if (retrievedTableTarget == null)
            {
                throw new NLogRuntimeException("Target 'flushedTableTarget' wrapped target is not of type 'TableTarget'.");
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
            Logger.Info($"Query results : {queryStopwatch.Elapsed.TotalMilliseconds.ToString(CultureInfo.InvariantCulture)} milliseconds.");

            var mergeStopwatch = new Stopwatch();
            mergeStopwatch.Start();
            var mergedResults = DataMerger.MergeResults(result);
            mergeStopwatch.Stop();
            Logger.Info($"Merged results : {mergeStopwatch.Elapsed.TotalMilliseconds.ToString(CultureInfo.InvariantCulture)} milliseconds.");

            var excelGenerationStopwatch = new Stopwatch();
            excelGenerationStopwatch.Start();
            ExcelExporter.Generate(mergedResults);
            excelGenerationStopwatch.Stop();
            Logger.Info($"Excel generation : {excelGenerationStopwatch.Elapsed.TotalMilliseconds.ToString(CultureInfo.InvariantCulture)} milliseconds.");
        }
    }
}
