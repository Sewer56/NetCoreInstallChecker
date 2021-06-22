using System;
using NetCoreInstallChecker.Structs.Config;
using NetCoreInstallChecker.Structs.Config.Enum;
using Xunit;
using Xunit.Abstractions;

namespace NetCoreInstallChecker.Tests
{
    /// <summary>
    /// Checks if the library can correctly resolve dependencies given a framework requirement.
    /// 
    /// Please note: All tests in here are manually performed.
    /// They will always pass and are meant to be double checked by the user.
    /// </summary>
    public class DependencyResolverTests
    {
        private readonly ITestOutputHelper _output;
        public DependencyResolverTests(ITestOutputHelper testOutputHelper) => _output = testOutputHelper;

        [Fact]
        public void ResolveDependencies32() => ResolveDependencies(false);

        [Fact]
        public void ResolveDependencies64() => ResolveDependencies(true);

        private void ResolveDependencies(bool is64Bit)
        {
            var finder    = new FrameworkFinder(is64Bit);
            var resolver  = new DependencyResolver(finder);
            var framework = new Framework("Microsoft.WindowsDesktop.App", "5.0.0");
            var result    = resolver.Resolve(new RuntimeOptions("netcoreapp5.0", framework, RollForwardPolicy.Minor));

            _output.WriteLine($"Result: {result.Available}");
            _output.WriteLine($"Missing: {result.MissingDependencies.Count}");
            _output.WriteLine($"Dependencies: {result.Dependencies.Count}");
            _output.WriteLine($"Download URL (Win, x86): {framework.GetWindowsDownloadUrl(Architecture.x86)}");
            _output.WriteLine($"Download URL (Win, x64): {framework.GetWindowsDownloadUrl(Architecture.Amd64)}");
            _output.WriteLine($"Download URL (Win, Arm64): {framework.GetWindowsDownloadUrl(Architecture.Arm64)}");
        }
    }
}
