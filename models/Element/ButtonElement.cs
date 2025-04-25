using System.Text.Json.Serialization;

namespace webbuilder.api.models
{
    public class ButtonElement : Element
    {
        [JsonPropertyName("buttonType")]
        public string ButtonType { get; set; } = string.Empty;

        [JsonPropertyName("element")]
        public FrameElement? Element { get; set; }
    }
}