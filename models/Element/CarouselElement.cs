using System.Text.Json.Serialization;

namespace webbuilder.api.models
{
    public class CarouselElement : Element
    {
        // Helper method for working with carousel settings
        public Setting? GetCarouselSettings()
        {
            return Settings.FirstOrDefault(s => s.SettingType == "carousel");
        }

        [JsonPropertyName("carouselSettings")]
        public Dictionary<string, object> CarouselSettings
        {
            get
            {
                var settings = GetCarouselSettings();
                return settings?.Settings ?? new Dictionary<string, object>();
            }
        }

        // Children will be used for the carousel elements
    }
}