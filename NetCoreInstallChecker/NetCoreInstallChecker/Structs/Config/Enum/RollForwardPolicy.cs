namespace NetCoreInstallChecker.Structs.Config.Enum
{
    /// <summary>
    /// RollForward specifies the roll-forward policy for an application, either as a fallback to accommodate
    /// missing a specific runtime version or as a directive to use a later version.
    /// </summary>
    public enum RollForwardPolicy
    {
        /// <summary>
        /// [Default Setting]
        /// Roll forward to the lowest higher minor version, if requested minor version is missing.
        /// If the requested minor version is present, then the LatestPatch policy is used.
        /// </summary>
        Minor,

        /// <summary>
        /// Roll forward to the highest patch version. This disables minor version roll forward.
        /// </summary>
        LatestPatch,

        /// <summary>
        /// Roll forward to lowest higher major version, and lowest minor version, if requested major version is missing.
        /// If the requested major version is present, then the Minor policy is used.
        /// </summary>
        Major,

        /// <summary>
        /// Roll forward to highest minor version, even if requested minor version is present.
        /// </summary>
        LatestMinor,

        /// <summary>
        /// Roll forward to highest major and highest minor version, even if requested major is present.
        /// </summary>
        LatestMajor,

        /// <summary>
        /// Do not roll forward. Only bind to specified version.
        /// This policy is not recommended for general use since it disables the ability to roll-forward to the latest patches.
        /// It is only recommended for testing.
        /// </summary>
        Disable,
    }
}