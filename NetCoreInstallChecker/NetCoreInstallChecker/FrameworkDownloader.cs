using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using NetCoreInstallChecker.Structs.Config.Enum;
using NuGet.Versioning;

namespace NetCoreInstallChecker
{
    /// <summary>
    /// An API allowing for easy downloading of individual frameworks.
    /// </summary>
    public struct FrameworkDownloader
    {
        private static readonly NuGetVersion _firstWindowsDesktopOutsideRuntimeVersion = new NuGetVersion("5.0.0");
        const string BaseUrl = "https://dotnetcli.azureedge.net/dotnet";

        /// <summary>
        /// The desired framework version.
        /// </summary>
        public NuGetVersion Version;

        /// <summary>
        /// The framework name/type.
        /// </summary>
        public FrameworkName FrameworkName;

        /// <summary/>
        /// <param name="version">The version of the API to download.</param>
        public FrameworkDownloader(NuGetVersion version, FrameworkName name)
        {
            Version = version;
            FrameworkName = name;
        }

        /// <summary>
        /// Tries to get latest patch version.
        /// </summary>
        /// <returns>Null if the operation failed, else the latest patch.</returns>
        public async Task<NuGetVersion> GetLatestPatchAsync()
        {
            var url = GetLatestPatchUrl();
            
            try
            {
                using var client = new HttpClient();
                var result = await client.GetStringAsync(new Uri(url));
                return new NuGetVersion(result);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Guesses the URL to the download for a given framework and version using the same method as the dotnet-install official script.
        /// Please note that there is no guarantee that the returned target URL exists.
        /// </summary>
        /// <param name="arch">The architecture of the CPU.</param>
        /// <param name="platform">The platform we are getting the download URL for.</param>
        /// <param name="format">The file format to download.</param>
        /// <param name="tryGetLatestPatch">If true, tries to get latest patch, i.e. 5.0.8 if 5.0.0 is requested.</param>
        public async Task<string> GetDownloadUrlAsync(Architecture arch, Platform platform = Platform.Windows, Format format = Format.Executable, bool tryGetLatestPatch = true)
        {
            var version = Version;
            if (tryGetLatestPatch)
            {
                var patch = await GetLatestPatchAsync();
                if (patch != null)
                    version = patch;
            }

            // Sanity checks.
            if (platform != Platform.Windows && FrameworkName == FrameworkName.WindowsDesktop)
                throw new NotSupportedException("Windows Desktop Framework is only supported on Windows.");

            // win, linux, osx
            var platformIdentifier = EnumExtensions.ToString(platform);
            var extension          = EnumExtensions.ToString(format, platform);
            var archIdentifier     = EnumExtensions.ToString(arch);
            var directory          = FrameworkName.ToDownloadDirectory();

            switch (FrameworkName)
            {
                case FrameworkName.App:
                    return BaseUrl + $"/{directory}/{version}/dotnet-runtime-{version}-{platformIdentifier}-{archIdentifier}.{extension}";
                case FrameworkName.Asp:
                    return BaseUrl + $"/{directory}/{version}/aspnetcore-runtime-{version}-{platformIdentifier}-{archIdentifier}.{extension}";
                case FrameworkName.WindowsDesktop:

                    // The windows desktop runtime is part of the core runtime layout prior to 5.0
                    if (version >= _firstWindowsDesktopOutsideRuntimeVersion)
                    {
                        return BaseUrl + $"/{directory}/{version}/windowsdesktop-runtime-{version}-{platformIdentifier}-{archIdentifier}.{extension}";
                    }
                    else
                    {
                        return BaseUrl + $"/{FrameworkName.App.ToDownloadDirectory()}/{version}/windowsdesktop-runtime-{version}-{platformIdentifier}-{archIdentifier}.{extension}";
                    }
                default:
                    throw new ArgumentOutOfRangeException("Unsupported framework for URL acquiring", (Exception)null);
            }
        }

        private string GetLatestPatchUrl()
        {
            var twoPartVersion = $"{Version.Major}.{Version.Minor}";
            var directory = FrameworkName.ToDownloadDirectory();

            switch (FrameworkName)
            {
                case FrameworkName.App:
                    return BaseUrl + $"/{directory}/{twoPartVersion}/latest.version";
                case FrameworkName.WindowsDesktop:

                    // The windows desktop runtime is part of the core runtime layout prior to 5.0
                    if (Version >= _firstWindowsDesktopOutsideRuntimeVersion)
                    {
                        return BaseUrl + $"/{directory}/{twoPartVersion}/latest.version";
                    }
                    else
                    {
                        return BaseUrl + $"/{FrameworkName.App.ToDownloadDirectory()}/{twoPartVersion}/latest.version";
                    }
                default:
                    throw new ArgumentOutOfRangeException("Unsupported framework for URL acquiring", (Exception)null);
            }
        }
    }
}
