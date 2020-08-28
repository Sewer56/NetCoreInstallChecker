using System.Collections.Generic;
using NetCoreInstallChecker.Policies;
using NuGet.Versioning;

namespace NetCoreInstallChecker.Interfaces
{
    public interface IRollForwardPolicy
    {
        /// <summary>
        /// Validates a given version against a list of supported versions using the current policy.
        /// Note: Does not return true if there is an exact version match unless policy is <see cref="Disable"/>
        /// Use <see cref="RollForwardPolicy.TryGetSupportedVersion(IRollForwardPolicy, NuGetVersion, NuGetVersion[], out NuGetVersion)"/> if you require this behaviour.
        /// </summary>
        /// <param name="version">The version to check.</param>
        /// <param name="versions">The supported versions.</param>
        /// <param name="supportedVersion">The supported version according to the policy.</param>
        bool TryGetSupportedVersion(NuGetVersion version, IEnumerable<NuGetVersion> versions,
            out NuGetVersion supportedVersion);
    }
}