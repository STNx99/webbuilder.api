using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace webbuilder.api.dtos
{
    public record class CreateElementDto
    {
        [JsonPropertyName("type")]
        [Required] public required string Type { get; init; }
        [JsonPropertyName("id")]
        [Required] public required string Id { get; init; }
        [JsonPropertyName("content")]
        public string? Content { get; init; }
        [JsonPropertyName("isSelected")]
        public bool IsSelected { get; init; }
        [JsonPropertyName("styles")]
        public Dictionary<string, object> Styles { get; init; } = []; // Fixed
        [JsonPropertyName("x")]
        public double X { get; init; }
        [JsonPropertyName("y")]
        public double Y { get; init; }
        [JsonPropertyName("src")]
        public string? Src { get; init; }
        [JsonPropertyName("href")]
        public string? Href { get; init; }
        [JsonPropertyName("parentId")]
        public string? ParentId { get; init; }
        [JsonPropertyName("projectId")]
        [Required] public required string ProjectId { get; init; }
    }
}