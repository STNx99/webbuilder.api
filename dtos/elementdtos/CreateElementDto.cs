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
        [JsonPropertyName("name")]
        public string? Name { get; init; }
        [JsonPropertyName("content")]
        public string? Content { get; init; }
        [JsonPropertyName("isSelected")]
        public bool IsSelected { get; init; }
        [JsonPropertyName("styles")]
        public Dictionary<string, object> Styles { get; init; } = [];
        [JsonPropertyName("tailwindStyles")]
        public string? TailwindStyles { get; init; }
        [JsonPropertyName("order")]
        public int Order { get; init; }
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
        [JsonPropertyName("buttonType")]
        public string? ButtonType { get; init; }
        [JsonPropertyName("projectId")]
        [Required] public required string ProjectId { get; init; }
        [JsonPropertyName("options")]
        public List<Dictionary<string, object>>? Options { get; init; }
        [JsonPropertyName("carouselSettings")]
        public Dictionary<string, object>? CarouselSettings { get; init; }

        [JsonPropertyName("selectSettings")]
        public Dictionary<string, object>? SelectSettings { get; init; }
        [JsonPropertyName("inputSettings")]
        public Dictionary<string, object>? InputSettings { get; init; }
    }
}