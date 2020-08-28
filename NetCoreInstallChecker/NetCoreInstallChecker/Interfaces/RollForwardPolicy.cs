using NetCoreInstallChecker.Policies;
using System;
using System.Collections.Generic;
using NuGet.Versioning;

namespace NetCoreInstallChecker.Interfaces
{
    public static class RollForwardPolicy
    {
        /// <summary>
        /// Validates a given version against a list of supported versions using the current policy.
        /// </summary>
        /// <param name="policy">The roll forward policy to use.</param>
        /// <param name="version">The version to check.</param>
        /// <param name="versions">The supported versions.</param>
        /// <param name="supportedVersion">The supported version according to the policy.</param>
        public static bool TryGetSupportedVersion(IRollForwardPolicy policy, NuGetVersion version, IEnumerable<NuGetVersion> versions, out NuGetVersion supportedVersion)
        {
            if (Disable.Instance.TryGetSupportedVersion(version, versions, out supportedVersion))
                return true;

            return policy.TryGetSupportedVersion(version, versions, out supportedVersion);
        }

        /// <summary>
        /// Retrieves a policy implementation for a given policy.
        /// </summary>
        /// <param name="policy">The policy to get an implementation for.</param>
        public static IRollForwardPolicy GetPolicy(Structs.Config.Enum.RollForwardPolicy policy)
        {
            switch (policy)
            {
                case Structs.Config.Enum.RollForwardPolicy.Minor:
                    return new Minor();
                case Structs.Config.Enum.RollForwardPolicy.LatestPatch:
                    return new LatestPatch();
                case Structs.Config.Enum.RollForwardPolicy.Major:
                    return new Major();
                case Structs.Config.Enum.RollForwardPolicy.LatestMinor:
                    return new LatestMinor();
                case Structs.Config.Enum.RollForwardPolicy.LatestMajor:
                    return new LatestMajor();
                case Structs.Config.Enum.RollForwardPolicy.Disable:
                    return new Disable();
                default:
                    throw new ArgumentOutOfRangeException(nameof(policy), policy, null);
            }
        }
    }
}
