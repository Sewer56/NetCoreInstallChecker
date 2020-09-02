using System;
using NuGet.Versioning;
using System.Collections.Generic;
using static System.Environment;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using NetCoreInstallChecker.Misc;
using NetCoreInstallChecker.Structs;
using NetCoreInstallChecker.Structs.Config;
using NetCoreInstallChecker.Structs.Config.Enum;

namespace NetCoreInstallChecker
{
    /// <summary>
    /// Discovers all versions of .NET Core installed on the machine for each target framework for a given architecture.
    /// </summary>
    public class FrameworkFinder
    {
        /// <summary>
        /// Maps given frameworks by name (e.g. Microsoft.NetCore.App to all available versions).
        /// </summary>
        private Dictionary<string, FrameworkOptionsTuple[]> _frameworks = new Dictionary<string, FrameworkOptionsTuple[]>(StringComparer.OrdinalIgnoreCase);

        public FrameworkFinder(bool is64Bit)
        {
            string installFolder = GetInstallFolder(is64Bit);
            var sharedFolder     = Path.Combine(installFolder, "dotnet/shared");
            if (!Directory.Exists(sharedFolder))
                return;

            var frameworkFolders = Directory.GetDirectories(sharedFolder);
            
            foreach (var folder in frameworkFolders)
            {
                var versions  = Directory.GetDirectories(folder);
                var framework = Path.GetFileName(folder);

                _frameworks[framework] = versions.Select(x =>
                {
                    var version = new NuGetVersion(Path.GetFileName(x));
                    var config  = Actions.TryGetValue(() => RuntimeOptions.FromFile($"{x}//{framework}.runtimeconfig.json"));
                    return new FrameworkOptionsTuple(new Framework(framework, version.ToString()), config);
                }).ToArray();
            }
        }

        /// <summary>
        /// Gets all of the available configs for a given framework.
        /// </summary>
        /// <param name="framework">The framework name.</param>
        public FrameworkOptionsTuple[] GetConfigs(FrameworkName framework) => GetConfigs(EnumExtensions.ToString(framework));

        /// <summary>
        /// Gets all of the available configs for a given framework.
        /// </summary>
        /// <param name="framework">The framework name.</param>
        public FrameworkOptionsTuple[] GetConfigs(string framework)
        {
            if (framework == null)
                return new FrameworkOptionsTuple[0];

            _frameworks.TryGetValue(framework, out var tuples);
            return tuples?.ToArray();
        }

        /// <summary>
        /// Retrieves all of the available frameworks.
        /// </summary>
        public string[] GetFrameworks() => _frameworks.Keys.ToArray();

        /// <summary>
        /// Retrieves the folder dotnet would be installed in.
        /// </summary>
        private static string GetInstallFolder(bool is64Bit)
        {
            string installFolder;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                installFolder = is64Bit ? ExpandEnvironmentVariables("%ProgramW6432%") : ExpandEnvironmentVariables(IntPtr.Size == 8 ? "%ProgramFiles(x86)%" : "%ProgramFiles%");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                installFolder = "/home/user";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                installFolder = "/usr/local/share";
            else
                throw new Exception("Unsupported OS, no known global install location.");

            return installFolder;
        }
    }
}
