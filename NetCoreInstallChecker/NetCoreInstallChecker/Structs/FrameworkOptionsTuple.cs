using System;
using NetCoreInstallChecker.Structs.Config;

namespace NetCoreInstallChecker.Structs
{
    public class FrameworkOptionsTuple : Tuple<Framework, RuntimeOptions>
    {
        public Framework Framework      => this.Item1;
        public RuntimeOptions Options   => this.Item2;

        /// <inheritdoc />
        public FrameworkOptionsTuple(Framework item1, RuntimeOptions item2) : base(item1, item2) { }

        /// <inheritdoc />
        public override string ToString() => $"{Framework} // {Options}";
    }
}
