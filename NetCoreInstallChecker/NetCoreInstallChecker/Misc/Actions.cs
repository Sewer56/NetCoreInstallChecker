using System;

namespace NetCoreInstallChecker.Misc
{
    public static class Actions
    {
        /// <summary>
        /// Tries to get a given value.
        /// </summary>
        /// <param name="function">The function to execute.</param>
        /// <returns>A value if applicable.</returns>
        public static T TryGetValue<T>(Func<T> function)
        {
            try
            {
                return function();
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
