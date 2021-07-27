using System;
using NetCoreInstallChecker.Structs.Config.Enum;
using NuGet.Versioning;
using Xunit;
using Xunit.Abstractions;

namespace NetCoreInstallChecker.Tests
{
    public class FrameworkDownloaderTests
    {
        private readonly ITestOutputHelper _output;
        public FrameworkDownloaderTests(ITestOutputHelper testOutputHelper) => _output = testOutputHelper;

        [Fact]
        public void GetDownloadUrls()
        {
            // Get all download URLs
            var downloader = new FrameworkDownloader(new NuGetVersion("5.0.0"), FrameworkName.App);
            foreach (var platform in Enum.GetValues<Platform>())
            {
                foreach (var arch in Enum.GetValues<Architecture>())
                {
                    foreach (var format in Enum.GetValues<Format>())
                    {
                        var label = $"Download URL ({platform}, {arch}, {format}): ";
                        try
                        {
                            label += downloader.GetDownloadUrlAsync(arch, platform, format).Result;
                        }
                        catch (Exception e)
                        {
                            if (e is AggregateException || e is NotSupportedException)
                                label += "Not Supported";
                        }

                        _output.WriteLine(label);
                    }
                }
            }
        }
    }
}
