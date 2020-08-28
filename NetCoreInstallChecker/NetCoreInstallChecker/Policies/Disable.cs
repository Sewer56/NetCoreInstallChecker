using System.Collections.Generic;
using NetCoreInstallChecker.Interfaces;
using NuGet.Versioning;

namespace NetCoreInstallChecker.Policies
{
    public class Disable : IRollForwardPolicy
    {
        public static Disable Instance = new Disable();

        public bool TryGetSupportedVersion(NuGetVersion version, IEnumerable<NuGetVersion> versions,
            out NuGetVersion supportedVersion)
        {
            supportedVersion = null;

            int major = version.Major;
            int minor = version.Minor;
            int patch = version.Patch;

            foreach (var ver in versions)
            {
                if (ver.Major == major && ver.Minor == minor && ver.Patch == patch)
                {
                    supportedVersion = ver;
                    return true;
                }
            }

            return false;
        }
    }
}
