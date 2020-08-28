using System.Collections.Generic;

namespace NetCoreInstallChecker.Structs
{
    public class DependencySearchResult<T>
    {
        /// <summary>
        /// Represents the resolved dependencies for the current item.
        /// </summary>
        public HashSet<T> Dependencies             { get; } = new HashSet<T>();

        /// <summary>
        /// Represents the missing dependencies for the current item.
        /// </summary>
        public HashSet<T> MissingDependencies      { get; } = new HashSet<T>();

        /// <summary>
        /// True if all dependencies are available.
        /// </summary>
        public bool Available => MissingDependencies.Count == 0;
    }
}
