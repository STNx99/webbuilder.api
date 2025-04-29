using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace webbuilder.api.models
{
    public class Setting
    {
        [Key]
        public required string Id { get; set; } = string.Empty;
        public required string Name { get; set; }
        [JsonPropertyName("settingType")]
        public required string SettingType { get; set; }
        [JsonPropertyName("settings")]
        public Dictionary<string, object> Settings { get; set; } = [];
        public string? ElementId { get; set; }
        [JsonIgnore]
        public Element? Element { get; set; }

    }
}