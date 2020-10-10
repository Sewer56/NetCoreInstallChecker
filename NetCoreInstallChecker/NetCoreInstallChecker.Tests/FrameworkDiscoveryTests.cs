using System;
using NetCoreInstallChecker.Structs.Config.Enum;
using Xunit;
using Xunit.Abstractions;

namespace NetCoreInstallChecker.Tests
{
    /// <summary>
    /// Checks if the library can correctly detect installed core versions for each framework.
    /// 
    /// Please note: All tests in here are manually performed.
    /// They will always pass and are meant to be double checked by the user.
    /// </summary>
    public class FrameworkDiscoveryTests
    {
        private readonly ITestOutputHelper _output;

        public FrameworkDiscoveryTests(ITestOutputHelper testOutputHelper) => _output = testOutputHelper;

        [Fact]
        public void GetAllVersions32() => GetAllVersions(false);

        [Fact]
        public void GetAllVersions64() => GetAllVersions(true);

        private void GetAllVersions(bool is64Bit)
        {
            var finder = new FrameworkFinder(is64Bit);

            foreach (var name in (FrameworkName[])Enum.GetValues(typeof(FrameworkName)))
            {
                var configs = finder.GetConfigs(name);
                _output.WriteLine($"Framework: " + name);
                _output.WriteLine($"Versions: ");
                Array.ForEach(configs, x => _output.WriteLine(x.Framework.Version.ToString()));
                _output.WriteLine($"Locations: ");
                Array.ForEach(configs, x => _output.WriteLine(x.FolderPath));
                _output.WriteLine("");
            }
        }
    }
}
