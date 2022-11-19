using System.Collections.Generic;
using NetCoreInstallChecker.Interfaces;
using NuGet.Versioning;

namespace NetCoreInstallChecker.Policies
{
    public class Major : IRollForwardPolicy
    {
        public bool TryGetSupportedVersion(NuGetVersion version, IEnumerable<NuGetVersion> versions,
            out NuGetVersion supportedVersion)
        {
            // Use LatestPatch policy if requested minor is present.
            if (Minor.Instance.TryGetSupportedVersion(version, versions, out supportedVersion))
                return true;

            int major = version.Major;
            supportedVersion = null;

            foreach (var ver in versions)
            {
                // Discard if incompatible.
                if (ver.Major < major)
                    continue;

                if (supportedVersion == null)
                    supportedVersion = ver;

                // Lowest major version.
                if (ver.Major < supportedVersion.Major)
                {
                    supportedVersion = ver;
                    continue;
                }
                else if (ver.Major == supportedVersion.Major)
                {
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
            }

            return supportedVersion != null && supportedVersion >= version;
        }
    }
}
