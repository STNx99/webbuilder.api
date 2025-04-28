using System.Text.Json.Serialization;

namespace webbuilder.api.models
{
    public class SelectElement : Element
    {
        // Helper methods for working with select settings
        public Setting? GetSelectSettings()
        {
            return Settings.FirstOrDefault(s => s.SettingType == "select");
        }

        [JsonPropertyName("selectSettings")]
        public Dictionary<string, object>? SelectSettings
        {
            get
            {
                var settings = GetSelectSettings();
                return settings?.Settings;
            }
        }

        // Keep Options as-is since it's specifically structured data for select elements
        [JsonPropertyName("options")]
        public List<Dictionary<string, object>> Options { get; set; } = [];
    }
}