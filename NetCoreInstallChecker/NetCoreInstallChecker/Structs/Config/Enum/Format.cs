namespace NetCoreInstallChecker.Structs.Config.Enum
{
    /// <summary>
    /// Defines the format of the downloaded file.
    /// </summary>
    public enum Format
    {
        /// <summary>
        /// File is an archive (zip or tar).
        /// </summary>
        Archive,

        /// <summary>
        /// File is an executable.
        /// </summary>
        Executable
    }
}