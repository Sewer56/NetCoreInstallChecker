using NuGet.Versioning;
using NetCoreInstallChecker.Policies;
using Xunit;
using NetCoreInstallChecker.Interfaces;

namespace NetCoreInstallChecker.Tests
{
    public class RollForwardPolicyTests
    {
        public static readonly NuGetVersion Version             = new NuGetVersion("3.0.0");
        public static readonly NuGetVersion UnsupportedMinor    = new NuGetVersion("3.3.0");
        public static readonly NuGetVersion UnsupportedPatch    = new NuGetVersion("3.0.1");
        public static readonly NuGetVersion UnsupportedMajor    = new NuGetVersion("2.0.0");

        public static NuGetVersion[] AvailableVersions => new NuGetVersion[]
        {
            new NuGetVersion("3.0.0"),
            new NuGetVersion("3.0.2"),
            new NuGetVersion("3.1.0"),
            new NuGetVersion("3.1.6"),
            new NuGetVersion("3.2.0"),

            new NuGetVersion("3.4.0"),
            new NuGetVersion("3.4.3"),
            new NuGetVersion("5.0.0-preview.7.20366.1"),
            new NuGetVersion("5.0.0"),
            new NuGetVersion("5.0.7"),
            new NuGetVersion("5.1.2")
        };

        [Fact]
        public void Minor()
        {
            var policy = new Minor();
            
            // Minor present, use LatestPatch
            Assert(policy, true, new NuGetVersion("3.0.2"), Version);
            Assert(policy, false, null, UnsupportedMajor);

            // Minor present, use LatestPatch
            Assert(policy, true, new NuGetVersion("3.0.2"), UnsupportedPatch);

            // Minor not present, upgrade.
            Assert(policy, true, new NuGetVersion("3.4.3"), UnsupportedMinor);
        }

        [Fact]
        public void LatestPatch()
        {
            var policy = new LatestPatch();
            Assert(policy, true, new NuGetVersion("3.0.2"), Version);
            Assert(policy, false, null, UnsupportedMajor);
            Assert(policy, true, new NuGetVersion("3.0.2"), UnsupportedPatch);
            Assert(policy, false, null, UnsupportedMinor);
        }

        [Fact]
        public void Major()
        {
            var policy = new Major();

            // Major present, use minor policy.
            Assert(policy, true, new NuGetVersion("3.0.2"), Version);

            // Major missing, roll forward to lowest higher major version, and lowest minor.
            Assert(policy, true, new NuGetVersion("3.0.2"), UnsupportedMajor);

            // Major present, use minor policy.
            Assert(policy, true, new NuGetVersion("3.0.2"), UnsupportedPatch);

            // Major present, use minor policy.
            Assert(policy, true, new NuGetVersion("3.4.3"), UnsupportedMinor);
        }

        [Fact]
        public void LatestMinor()
        {
            var policy = new LatestMinor();
            Assert(policy, true, new NuGetVersion("3.4.3"), Version);
            Assert(policy, false, null, UnsupportedMajor);
            Assert(policy, true, new NuGetVersion("3.4.3"), UnsupportedPatch);
            Assert(policy, true, new NuGetVersion("3.4.3"), UnsupportedMinor);
        }

        [Fact]
        public void LatestMajor()
        {
            var policy = new LatestMajor();
            Assert(policy, true, new NuGetVersion("5.1.2"), Version);
            Assert(policy, true, new NuGetVersion("5.1.2"), UnsupportedMajor);
            Assert(policy, true, new NuGetVersion("5.1.2"), UnsupportedPatch);
            Assert(policy, true, new NuGetVersion("5.1.2"), UnsupportedMinor);
        }

        [Fact]
        public void Disable()
        {
            var policy = new Disable();
            Assert(policy, true, new NuGetVersion("3.0.0"), Version);
            Assert(policy, false, null, UnsupportedMajor);
            Assert(policy, false, null, UnsupportedPatch);
            Assert(policy, false, null, UnsupportedMinor);
        }

        private static void Assert(IRollForwardPolicy policy, bool isSupported, NuGetVersion expected, NuGetVersion version)
        {
            bool supported = policy.TryGetSupportedVersion(version, AvailableVersions, out var ver);
            Xunit.Assert.Equal(isSupported, supported);
            Xunit.Assert.Equal(expected, ver);
        }
    }
}
