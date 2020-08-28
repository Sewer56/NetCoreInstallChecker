using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreInstallChecker.Structs.Config.Enum
{
    public static class EnumExtensions
    {
        private static Dictionary<string, FrameworkName> _frameworkStringToNameMap = new Dictionary<string, FrameworkName>(StringComparer.OrdinalIgnoreCase)
        {
            { "Microsoft.NETCore.App" , FrameworkName.App },
            { "Microsoft.AspNetCore.App" , FrameworkName.Asp },
            { "Microsoft.WindowsDesktop.App" , FrameworkName.WindowsDesktop },
        };

        private static Dictionary<FrameworkName, string> _frameworkNameToStringMap = _frameworkStringToNameMap.ToDictionary((i) => i.Value, (i) => i.Key);

        /// <summary>
        /// Converts a given string to a framework name by using case insensitive comparison.
        /// </summary>
        /// <param name="name">Name of a framework.</param>
        /// <returns><see cref="FrameworkName"/> for the given name; otherwise <see cref="FrameworkName.Null"/> is unknown framework.</returns>
        public static FrameworkName ToFrameworkName(this string name)
        {
            _frameworkStringToNameMap.TryGetValue(name, out var value);
            return value;
        }

        /// <summary>
        /// Converts a given framework name to a string.
        /// </summary>
        /// <param name="name">The framework.</param>
        /// <returns>String for the given enum; otherwise empty string if it is an unknown framework.</returns>
        public static string ToString(this FrameworkName name)
        {
            _frameworkNameToStringMap.TryGetValue(name, out var value);
            return value;
        }
    }
}
