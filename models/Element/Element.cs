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
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("isSelected")]
        public bool IsSelected { get; set; }

        [JsonPropertyName("styles")]
        public Dictionary<string, object> Styles { get; set; } = [];

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("x")]
        public double X { get; set; }

        [JsonPropertyName("y")]
        public double Y { get; set; }

        [JsonPropertyName("src")]
        public string? Src { get; set; }

        [JsonPropertyName("href")]
        public string? Href { get; set; }

        [JsonPropertyName("parentId")]
        public string? ParentId { get; set; }

        [JsonPropertyName("projectId")]
        public required string ProjectId { get; set; }
    }
}