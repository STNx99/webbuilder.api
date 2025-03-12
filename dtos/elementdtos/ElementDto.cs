using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace webbuilder.api.dtos
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
    [JsonDerivedType(typeof(TextElementDto), typeDiscriminator: "Text")]
    [JsonDerivedType(typeof(AElementDto), typeDiscriminator: "A")]
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
        public bool IsSelected { get; init; } = false;
        [JsonPropertyName("styles")]
        public Dictionary<string, object> Styles { get; init; } = [];

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
    public record class TextElementDto : ElementDto
    {

    }

    public record class AElementDto : ElementDto
    {

    }

    public record class FrameElementDto : ElementDto
    {
        [JsonPropertyName("elements")]
        public List<ElementDto> Elements { get; init; } = new List<ElementDto>();
    }
}