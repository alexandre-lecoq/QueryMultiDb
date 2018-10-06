using System.IO;

namespace QueryMultiDb.Tests.System
{
    public static class ExecutableResolver
    {
        private const string QueryMultiDbFilename = "QueryMultiDb.exe";
        private const string RelativeExecutablePath = @"\..\..\..\QueryMultiDb\bin\";
        private const string DebugBuildPath = "Debug";
        private const string ReleaseBuildPath = "Release";
        private const string BuildTypePattern = "<BuildTypePattern>";

        public static string GetQueryMultiDbExecutablePath()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var buildType = GetBuildType(currentDirectory);
            var neutralCurrentDirectory = currentDirectory
                .Replace(DebugBuildPath, BuildTypePattern)
                .Replace(ReleaseBuildPath, BuildTypePattern);
            var neutralSuspectedPath = neutralCurrentDirectory +
                                       RelativeExecutablePath +
                                       BuildTypePattern +
                                       Path.DirectorySeparatorChar +
                                       QueryMultiDbFilename;
            var suspectedPath = neutralSuspectedPath.Replace(BuildTypePattern, buildType.ToString());

            if (!File.Exists(suspectedPath))
                throw new FileNotFoundException(
                    $"File could not be found. Current working directory : '{currentDirectory}'. Build type : '{buildType}'.",
                    suspectedPath);

            return suspectedPath;
        }

        private enum BuildType
        {
            Debug,
            Release,
            Unknown
        };

        private static BuildType GetBuildType(string path)
        {
            if (path.Contains(DebugBuildPath))
                return BuildType.Debug;

            if (path.Contains(ReleaseBuildPath))
                return BuildType.Release;

            return BuildType.Unknown;
        }
    }
}
