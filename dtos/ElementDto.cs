using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace webbuilder.api.dtos
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
    [JsonDerivedType(typeof(ElementDto), typeDiscriminator: "Text")]
    [JsonDerivedType(typeof(FrameElementDto), typeDiscriminator: "Frame")]
    public record class ElementDto
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
        public Dictionary<string, string> Styles { get; init; } = new Dictionary<string, string>();
        [JsonPropertyName("x")]
        public int X { get; init; }
        [JsonPropertyName("y")]
        public int Y { get; init; }
        [JsonPropertyName("src")]
        public string? Src { get; init; }
        [JsonPropertyName("href")]
        public string? Href { get; init; }
    }

    public record class FrameElementDto : ElementDto
    {
        [JsonPropertyName("elements")]
        public List<ElementDto> Elements { get; init; } = new List<ElementDto>();
    }
}