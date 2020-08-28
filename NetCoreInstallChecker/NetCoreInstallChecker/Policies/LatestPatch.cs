using System.Collections.Generic;
using NetCoreInstallChecker.Interfaces;
using NuGet.Versioning;

namespace NetCoreInstallChecker.Policies
{
    public class LatestPatch : IRollForwardPolicy
    {
        public static LatestPatch Instance = new LatestPatch();

        public bool TryGetSupportedVersion(NuGetVersion version, IEnumerable<NuGetVersion> versions,
            out NuGetVersion supportedVersion)
        {
            int major = version.Major;
            int minor = version.Minor;
            supportedVersion = null;

            foreach (var ver in versions)
            {
                // Discard if incompatible.
                if (ver.Major != major || ver.Minor != minor)
                    continue;

                if (supportedVersion == null)
                    supportedVersion = ver;

                // Latest patch.
                if (ver.Patch > supportedVersion.Patch)
                    supportedVersion = ver;
            }

            return supportedVersion != null;
        }
    }
}
