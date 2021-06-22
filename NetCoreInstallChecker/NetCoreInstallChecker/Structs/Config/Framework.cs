using System;
using System.Text.Json.Serialization;
using NetCoreInstallChecker.Structs.Config.Enum;
using NuGet.Versioning;
using FrameworkName = NetCoreInstallChecker.Structs.Config.Enum.FrameworkName;

namespace NetCoreInstallChecker.Structs.Config
{
    public class Framework
    {
        /// <summary>
        /// Contains the name of the framework, e.g. Microsoft.WindowsDesktop.App
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Contains the version of the framework e.g. 3.0.0
        /// </summary>
        [JsonPropertyName("version")]
        public string Version { get; set; }

        public Framework(string name, string version)
        {
            Name = name;
            Version = version;
        }

        public Framework() { }

        /// <summary>
        /// Gets the framework name for the given string name.
        /// </summary>
        public FrameworkName FrameworkName => Name.ToFrameworkName();

        /// <summary>
        /// Converts the version string into a NuGet version.
        /// </summary>
        public NuGetVersion NuGetVersion => new NuGetVersion(Version);

        /// <summary>
        /// Gets the URL to the download for a given framework and version targeting Windows.
        /// </summary>
        public string GetWindowsDownloadUrl(Architecture arch)
        {
            var baseUrl = "https://dotnetcli.azureedge.net/dotnet";
            switch (FrameworkName)
            {
                case FrameworkName.App:
                    return baseUrl + $"/Runtime/{Version}/dotnet-runtime-{Version}-win-{EnumExtensions.ToString(arch)}.zip";
                case FrameworkName.Asp:
                    return baseUrl + $"/aspnetcore/Runtime/{Version}/aspnetcore-runtime-{Version}-win-{EnumExtensions.ToString(arch)}.zip";
                case FrameworkName.WindowsDesktop:
                    // The windows desktop runtime is part of the core runtime layout prior to 5.0
                    if (NuGetVersion >= new NuGetVersion("5.0.0"))
                    {
                        return baseUrl + $"/WindowsDesktop/{Version}/windowsdesktop-runtime-{Version}-win-{EnumExtensions.ToString(arch)}.zip";
                    }
                    else
                    {
                        return baseUrl + $"/Runtime/{Version}/windowsdesktop-runtime-{Version}-win-{EnumExtensions.ToString(arch)}.zip";
                    }
                default:
                    throw new ArgumentOutOfRangeException("Unsupported framework for URL acquiring", (Exception) null);
            }
        }

        /// <summary>
        /// Gets the URL to the install page for a given framework and version.
        /// </summary>
        public string GetInstallUrl()
        {
            return $"https://aka.ms/dotnet-core-applaunch?framework={Name}&framework_version={Version}";
        }

        /// <inheritdoc />
        public override string ToString() => Name;
    }
}
