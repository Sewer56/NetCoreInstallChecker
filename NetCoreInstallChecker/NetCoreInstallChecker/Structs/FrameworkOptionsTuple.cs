using System;
using NetCoreInstallChecker.Structs.Config;

namespace NetCoreInstallChecker.Structs
{
    public class FrameworkOptionsTuple : Tuple<Framework, RuntimeOptions, string>
    {
        /// <summary>
        /// Contains details such as the name and version of the framework.
        /// </summary>
        public Framework Framework      => this.Item1;

        /// <summary>
        /// [Optional] Missing if dependency is missing or no config file exists.
        /// </summary>
        public RuntimeOptions Options   => this.Item2;

        /// <summary>
        /// [Optional] Contains the full path to the folder the framework is contained in.
        /// Missing if dependency is missing or no config file exists.
        /// </summary>
        public string FolderPath        => this.Item3;

        /// <inheritdoc />
        public FrameworkOptionsTuple(Framework item1, RuntimeOptions item2, string item3) : base(item1, item2, item3) { }

        /// <inheritdoc />
        public override string ToString() => $"{Framework} // {Options}";
    }
}
