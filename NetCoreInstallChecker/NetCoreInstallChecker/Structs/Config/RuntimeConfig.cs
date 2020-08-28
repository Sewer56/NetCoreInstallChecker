using System.Text.Json.Serialization;

namespace NetCoreInstallChecker.Structs.Config
{
    /// <summary>
    /// Represents an individual runtime configuration.
    /// </summary>
    public class RuntimeConfig
    {
        [JsonPropertyName("runtimeOptions")]
        public RuntimeOptions RuntimeOptions { get; set; } = new RuntimeOptions();
    }
}
