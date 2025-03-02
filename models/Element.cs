using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace webbuilder.api.models
{
    public class Element
    {
        [Key]
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [Required]
        [JsonPropertyName("type")]
        public required string Type { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("isSelected")]
        public bool IsSelected { get; set; }

        [JsonPropertyName("styles")]
        public Dictionary<string, string> Styles { get; set; } = new();

        [JsonPropertyName("x")]
        public int X { get; set; }

        [JsonPropertyName("y")]
        public int Y { get; set; }

        [JsonPropertyName("src")]
        public string? Src { get; set; }

        [JsonPropertyName("href")]
        public string? Href { get; set; }

        [JsonPropertyName("parentId")]
        public string? ParentId { get; set; }
    }
}