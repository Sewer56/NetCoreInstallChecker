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

        /// <inheritdoc />
        public override string ToString() => Name;
    }
}
