using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreInstallChecker.Structs.Config.Enum
{
    public static class EnumExtensions
    {
        private static Dictionary<string, FrameworkName> _frameworkStringToNameMap = new Dictionary<string, FrameworkName>(StringComparer.OrdinalIgnoreCase)
        {
            { "Microsoft.NETCore.App" , FrameworkName.App },
            { "Microsoft.AspNetCore.App" , FrameworkName.Asp },
            { "Microsoft.WindowsDesktop.App" , FrameworkName.WindowsDesktop },
        };

        private static Dictionary<FrameworkName, string> _frameworkNameToStringMap = _frameworkStringToNameMap.ToDictionary((i) => i.Value, (i) => i.Key);

        /// <summary>
        /// Converts a given string to a framework name by using case insensitive comparison.
        /// </summary>
        /// <param name="name">Name of a framework.</param>
        /// <returns><see cref="FrameworkName"/> for the given name; otherwise <see cref="FrameworkName.Null"/> is unknown framework.</returns>
        public static FrameworkName ToFrameworkName(this string name)
        {
            _frameworkStringToNameMap.TryGetValue(name, out var value);
            return value;
        }

        /// <summary>
        /// Converts a given framework name to a string.
        /// </summary>
        /// <param name="name">The framework.</param>
        /// <returns>String for the given enum; otherwise empty string if it is an unknown framework.</returns>
        public static string ToString(this FrameworkName name)
        {
            _frameworkNameToStringMap.TryGetValue(name, out var value);
            return value;
        }

        /// <summary>
        /// Gets the name used in download URLs for a specific platform.
        /// </summary>
        /// <param name="platform">The platform to get the url for.</param>
        public static string ToString(this Platform platform)
        {
            return platform switch
            {
                Platform.Windows => "win",
                Platform.Linux => "linux",
                Platform.OSX => "osx",
                Platform.LinuxMusl => "linux-musl",
                _ => throw new ArgumentOutOfRangeException(nameof(platform), platform, null)
            };
        }

        /// <summary>
        /// Gets the extension used in download URLs for a specific platform.
        /// </summary>
        /// <param name="format">The format to get the extension for.</param>
        /// <param name="platform">The platform to get the extension for.</param>
        public static string ToString(this Format format, Platform platform)
        {
            switch (platform)
            {
                case Platform.Windows:
                    return format switch
                    {
                        Format.Archive => "zip",
                        Format.Executable => "exe",
                        _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
                    };
                case Platform.Linux:
                case Platform.LinuxMusl:
                    return format switch
                    {
                        Format.Archive => "tar.gz",
                        Format.Executable => throw new NotSupportedException("Linux platform with executable format is not supported."),
                        _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
                    };
                case Platform.OSX:
                    return format switch
                    {
                        Format.Archive => "tar.gz",
                        Format.Executable => "pkg",
                        _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(platform), platform, null);
            }
        }

        /// <summary>
        /// Gets the name identifier used in download URLs for a particular architecture.
        /// </summary>
        /// <param name="arch">The architecture to get identifier for.</param>
        public static string ToString(this Architecture arch)
        {
            return arch switch
            {
                Architecture.Amd64 => "x64",
                Architecture.x86 => "x86",
                Architecture.Arm => "arm",
                Architecture.Arm64 => "arm64",
                _ => throw new NotImplementedException("Unknown architecture"),
            };
        }

        /// <summary>
        /// Gets the download directory for the provided framework name.
        /// </summary>
        /// <param name="frameworkName">The framework.</param>
        public static string ToDownloadDirectory(this FrameworkName frameworkName)
        {
            return frameworkName switch
            {
                FrameworkName.App => "Runtime",
                FrameworkName.Asp => "aspnetcore/Runtime",
                FrameworkName.WindowsDesktop => "WindowsDesktop",
                _ => throw new ArgumentOutOfRangeException(nameof(frameworkName), frameworkName, null)
            };
        }
    }
}
