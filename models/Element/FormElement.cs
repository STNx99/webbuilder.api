using System.Text.Json.Serialization;

namespace webbuilder.api.models
{
    public class FormElement : Element
    {
        public Setting? GetFormSettings()
        {
            return Settings.FirstOrDefault(s => s.SettingType == "form");
        }

        [JsonPropertyName("formSettings")]
        public Dictionary<string, object> FormSettings
        {
            get
            {
                var settings = GetFormSettings();
                return settings?.Settings ?? [];
            }
        }
    }
}