using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace webbuilder.api.dtos
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
    [JsonDerivedType(typeof(TextElementDto), typeDiscriminator: "Text")]
    [JsonDerivedType(typeof(ImageElementDto), typeDiscriminator: "Image")]
    [JsonDerivedType(typeof(ButtonElementDto), typeDiscriminator: "Button")]
    [JsonDerivedType(typeof(LinkElementDto), typeDiscriminator: "Link")]
    [JsonDerivedType(typeof(FrameElementDto), typeDiscriminator: "Frame")]
    [JsonDerivedType(typeof(CarouselElementDto), typeDiscriminator: "Carousel")]
    [JsonDerivedType(typeof(InputElementDto), typeDiscriminator: "Input")]
    [JsonDerivedType(typeof(ListElementDto), typeDiscriminator: "ListItem")]
    [JsonDerivedType(typeof(SelectElementDto), typeDiscriminator: "Select")]
    public record class ElementDto
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
        public bool IsSelected { get; init; } = false;
        [JsonPropertyName("styles")]
        public Dictionary<string, object> Styles { get; init; } = [];
        [JsonPropertyName("tailwindStyles")]
        public string? TailwindStyles { get; init; }
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
    public record class ImageElementDto : ElementDto
    {

    }

    public record class LinkElementDto : ElementDto
    {

    }
    public record class ButtonElementDto : ElementDto
    {
        [JsonPropertyName("buttonType")]
        public string? ButtonType { get; init; }
    }

    public record class CarouselElementDto : ElementDto
    {
        [JsonPropertyName("elements")]
        public List<ElementDto> Elements { get; init; } = [];
        [JsonPropertyName("carouselSettings")]
        public Dictionary<string, object>? CarouselSettings { get; init; }
    }

    public record class FrameElementDto : ElementDto
    {
        [JsonPropertyName("elements")]
        public List<ElementDto> Elements { get; init; } = [];
    }

    public record class InputElementDto : ElementDto
    {
        [JsonPropertyName("inputSettings")]
        public Dictionary<string, object> InputSettings { get; init; } = [];
    }

    public record class ListElementDto : ElementDto
    {
        [JsonPropertyName("elements")]
        public List<ElementDto> Elements { get; init; } = [];
    }

    public record class SelectElementDto : ElementDto
    {
        [JsonPropertyName("options")]
        public List<Dictionary<string, object>> Options { get; init; } = [];

        [JsonPropertyName("selectSettings")]
        public Dictionary<string, object>? SelectSettings { get; init; }
    }
}