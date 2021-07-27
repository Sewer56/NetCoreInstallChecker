using System;
using System.Collections.Generic;
using NetCoreInstallChecker.Structs.Config;
using NetCoreInstallChecker.Structs.Config.Enum;
using NuGet.Versioning;
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
        }

        [Fact]
        public void ResolveDependenciesMulti()
        {
            var finder = new FrameworkFinder(true);
            var resolver = new DependencyResolver(finder);
            var frameworks = new List<Framework>()
            {
                new Framework("Microsoft.NETCore.App", "6.0.0-preview.6.21352.12"),
                new Framework("Microsoft.WindowsDesktop.App", "6.0.0-preview.6.21353.1")
            };

            var result = resolver.Resolve(new RuntimeOptions("net6.0", frameworks, RollForwardPolicy.Minor));
            _output.WriteLine($"Result: {result.Available}");
            _output.WriteLine($"Missing: {result.MissingDependencies.Count}");
            _output.WriteLine($"Dependencies: {result.Dependencies.Count}");
        }
    }
}
