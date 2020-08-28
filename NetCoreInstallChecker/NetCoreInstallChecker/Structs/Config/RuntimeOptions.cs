using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using NetCoreInstallChecker.Structs.Config.Enum;

namespace NetCoreInstallChecker.Structs.Config
{
    public class RuntimeOptions
    {
        private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            Converters = { new JsonStringEnumConverter() },
        };

        /// <summary>
        /// Represents the target framework such as netcoreapp3.0.
        /// </summary>
        [JsonPropertyName("tfm")]
        public string Tfm { get; set; }

        /// <summary>
        /// Contains the required framework name (e.g. Microsoft.WindowsDesktop.App) and version.
        /// </summary>
        [JsonPropertyName("framework")]
        public Framework Framework { get; set; }

        /// <summary>
        /// The policy that defines are safe to pick.
        /// </summary>
        [JsonPropertyName("rollForward")]
        public RollForwardPolicy RollForward { get; set; }

        public RuntimeOptions(string tfm, Framework framework, RollForwardPolicy rollForward)
        {
            Tfm = tfm;
            Framework = framework;
            RollForward = rollForward;
        }

        public RuntimeOptions() { }

        /// <summary>
        /// Parses a runtime configuration from physical disk.
        /// </summary>
        /// <param name="filePath">Physical path to the file.</param>
        public static RuntimeOptions FromFile(string filePath)
        {
            return File.Exists(filePath) ? FromJson(File.ReadAllText(filePath)) : null;
        }

        /// <summary>
        /// Parses a runtime configuration (runtimeconfig.json) from text.
        /// </summary>
        /// <param name="json">Serialized text data.</param>
        public static RuntimeOptions FromJson(string json)
        {
            return JsonSerializer.Deserialize<RuntimeConfig>(json, _jsonSerializerOptions).RuntimeOptions;
        }

        /// <inheritdoc />
        public override string ToString() => $"{Tfm} // {Framework}";
    }
}
