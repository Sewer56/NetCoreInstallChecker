using System.Collections.Generic;
using NetCoreInstallChecker.Interfaces;
using NuGet.Versioning;

namespace NetCoreInstallChecker.Policies
{
    public class LatestMinor : IRollForwardPolicy
    {
        public static LatestMinor Instance = new LatestMinor();

        public bool TryGetSupportedVersion(NuGetVersion version, IEnumerable<NuGetVersion> versions,
            out NuGetVersion supportedVersion)
        {
            int major = version.Major;
            int minor = version.Minor;
            supportedVersion = null;

            foreach (var ver in versions)
            {
                // Discard if incompatible.
                if (ver.Major != major || ver.Minor < minor)
                    continue;

                if (supportedVersion == null)
                    supportedVersion = ver;

                // Highest minor version.
                if (ver.Minor > supportedVersion.Minor)
                {
                    supportedVersion = ver;
                    continue;
                }

                if (ver.Patch > supportedVersion.Patch)
                    supportedVersion = ver;
            }

            return supportedVersion != null && supportedVersion >= version;
        }
    }
}
