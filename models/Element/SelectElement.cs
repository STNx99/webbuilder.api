using System.Text.Json.Serialization;

namespace webbuilder.api.models
{
    public class SelectElement : Element
    {
        [JsonPropertyName("options")]
        public List<Dictionary<string, object>> Options { get; set; } = [];

        [JsonPropertyName("selectSettings")]
        public Dictionary<string, object>? SelectSettings { get; set; }
    }
}