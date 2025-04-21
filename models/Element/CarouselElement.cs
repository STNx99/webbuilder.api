using System.Text.Json.Serialization;

namespace webbuilder.api.models
{
    public class CarouselElement : Element
    {
        [JsonPropertyName("settings")]
        public Dictionary<string, object> Settings { get; set; } = new Dictionary<string, object>();

        // Children will be used for the carousel elements
    }
}