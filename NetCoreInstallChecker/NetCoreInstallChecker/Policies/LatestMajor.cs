using System.Collections.Generic;
using NetCoreInstallChecker.Interfaces;
using NuGet.Versioning;

namespace NetCoreInstallChecker.Policies
{
    public class LatestMajor : IRollForwardPolicy
    {
        public bool TryGetSupportedVersion(NuGetVersion version, IEnumerable<NuGetVersion> versions,
            out NuGetVersion supportedVersion)
        {
            int major = version.Major;
            supportedVersion = null;

            foreach (var ver in versions)
            {
                // Discard if incompatible.
                if (ver.Major < major)
                    continue;

                if (supportedVersion == null)
                    supportedVersion = ver;

                // Highest major version.
                if (ver.Major > supportedVersion.Major)
                {
                    supportedVersion = ver;
                    continue;
                }

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
