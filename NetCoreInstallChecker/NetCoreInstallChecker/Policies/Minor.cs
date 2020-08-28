using System.Collections.Generic;
using NetCoreInstallChecker.Interfaces;
using NuGet.Versioning;

namespace NetCoreInstallChecker.Policies
{
    public class Minor : IRollForwardPolicy
    {
        public static Minor Instance = new Minor();

        public bool TryGetSupportedVersion(NuGetVersion version, IEnumerable<NuGetVersion> versions,
            out NuGetVersion supportedVersion)
        {
            // Use LatestPatch policy if requested minor is present.
            if (LatestPatch.Instance.TryGetSupportedVersion(version, versions, out supportedVersion))
                return true;

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

                // Lowest minor version.
                if (ver.Minor < supportedVersion.Minor)
                {
                    supportedVersion = ver;
                    continue;
                }
                else if (ver.Minor == supportedVersion.Minor)
                {
                    if (ver.Patch > supportedVersion.Patch)
                        supportedVersion = ver;
                }
            }

            return supportedVersion != null;
        }
    }
}
