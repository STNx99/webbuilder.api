using System.Text.Json.Serialization;

namespace webbuilder.api.models
{
    public class CarouselElement : Element
    {
        [JsonPropertyName("carouselSettings")]
        public Dictionary<string, object> CarouselSettings { get; set; } = new Dictionary<string, object>();

        // Children will be used for the carousel elements
    }
}