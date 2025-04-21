using System.Text.Json.Serialization;

namespace webbuilder.api.models
{
    public class InputElement : Element
    {
        [JsonPropertyName("inputSettings")]
        public Dictionary<string, object> InputSettings { get; set; } = [];
    }
}