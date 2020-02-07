using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QueryMultiDb.Common
{
    public static class ExecutableResolver
    {
        private const string QueryMultiDbFilename = "QueryMultiDb.exe";
        private const string DebugBuildPath = "Debug";
        private const string ReleaseBuildPath = "Release";
        private const string BuildTypePattern = "<BuildTypePattern>";

        public static string GetQueryMultiDbExecutablePath(string queryMultiDbFilename = null, string relativeExecutablePath = null, bool searchEnvironmentPath = false)
        {
            var pathsToSearch = new List<string>(25);
            var currentDirectory = Directory.GetCurrentDirectory();

            if (!string.IsNullOrWhiteSpace(relativeExecutablePath))
            {
                var neutralCurrentDirectory = currentDirectory
                    .Replace(DebugBuildPath, BuildTypePattern)
                    .Replace(ReleaseBuildPath, BuildTypePattern);
                var neutralPathPattern = neutralCurrentDirectory + relativeExecutablePath + BuildTypePattern;
                var currentDirectoryBuildType = GetBuildType(currentDirectory);
                pathsToSearch.Add(neutralPathPattern.Replace(BuildTypePattern, currentDirectoryBuildType.ToString()));
                pathsToSearch.Add(neutralPathPattern.Replace(BuildTypePattern, BuildType.Debug.ToString()));
                pathsToSearch.Add(neutralPathPattern.Replace(BuildTypePattern, BuildType.Release.ToString()));
            }

            if (searchEnvironmentPath)
            {
                var pathEnvironmentVariable = Environment.GetEnvironmentVariable("PATH");

                if (pathEnvironmentVariable != null)
                {
                    var pathsArray = pathEnvironmentVariable.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
                    var filteredPaths = pathsArray
                        .Select(p => p.Trim())
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .Where(Directory.Exists);
                    pathsToSearch.AddRange(filteredPaths);
                }
            }

            if (string.IsNullOrWhiteSpace(queryMultiDbFilename))
                queryMultiDbFilename = QueryMultiDbFilename;

            foreach (var path in pathsToSearch)
            {
                var absolutePath = Path.Combine(path, queryMultiDbFilename);

                if (File.Exists(absolutePath))
                    return absolutePath;
            }

            var message =
                $"File could not be found. queryMultiDbFilename = '{queryMultiDbFilename}'. relativeExecutablePath = '{relativeExecutablePath}'. searchEnvironmentPath = '{searchEnvironmentPath}'. Current working directory : '{currentDirectory}'.";
            throw new FileNotFoundException(message);
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
