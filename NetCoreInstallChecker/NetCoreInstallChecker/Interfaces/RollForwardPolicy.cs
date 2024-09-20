using NetCoreInstallChecker.Policies;
using System;
using System.Collections.Generic;
using NuGet.Versioning;

namespace NetCoreInstallChecker.Interfaces;

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
        // ReSharper disable once PossibleMultipleEnumeration
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
        return policy switch
        {
            Structs.Config.Enum.RollForwardPolicy.Minor => new Minor(),
            Structs.Config.Enum.RollForwardPolicy.LatestPatch => new LatestPatch(),
            Structs.Config.Enum.RollForwardPolicy.Major => new Major(),
            Structs.Config.Enum.RollForwardPolicy.LatestMinor => new LatestMinor(),
            Structs.Config.Enum.RollForwardPolicy.LatestMajor => new LatestMajor(),
            Structs.Config.Enum.RollForwardPolicy.Disable => new Disable(),
            _ => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
        };
    }
}