using System.Collections.Generic;

namespace NetCoreInstallChecker.Structs
{
    public class DependencySearchResult<TFound, TNotFound>
    {
        /// <summary>
        /// Represents the resolved dependencies for the current item.
        /// </summary>
        public HashSet<TFound> Dependencies             { get; } = new HashSet<TFound>();

        /// <summary>
        /// Represents the missing dependencies for the current item.
        /// </summary>
        public HashSet<TNotFound> MissingDependencies   { get; } = new HashSet<TNotFound>();

        /// <summary>
        /// True if all dependencies are available.
        /// </summary>
        public bool Available => MissingDependencies.Count == 0;
    }
}
