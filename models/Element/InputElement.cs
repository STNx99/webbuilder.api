using System.Text.Json.Serialization;

namespace webbuilder.api.models
{
    public class InputElement : Element
    {
        // Helper methods for working with input settings
        public Setting? GetInputSettings()
        {
            return Settings.FirstOrDefault(s => s.SettingType == "input");
        }

        [JsonPropertyName("inputSettings")]
        public Dictionary<string, object> InputSettings
        {
            get
            {
                var settings = GetInputSettings();
                return settings?.Settings ?? new Dictionary<string, object>();
            }
        }
    }
}