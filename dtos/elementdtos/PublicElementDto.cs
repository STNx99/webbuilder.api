using System.Text.Json.Serialization;

namespace webbuilder.api.dtos
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
    [JsonDerivedType(typeof(PublicTextElementDto), typeDiscriminator: "Text")]
    [JsonDerivedType(typeof(PublicImageElementDto), typeDiscriminator: "Image")]
    [JsonDerivedType(typeof(PublicButtonElementDto), typeDiscriminator: "Button")]
    [JsonDerivedType(typeof(PublicLinkElementDto), typeDiscriminator: "Link")]
    [JsonDerivedType(typeof(PublicFrameElementDto), typeDiscriminator: "Frame")]
    [JsonDerivedType(typeof(PublicCarouselElementDto), typeDiscriminator: "Carousel")]
    [JsonDerivedType(typeof(PublicInputElementDto), typeDiscriminator: "Input")]
    [JsonDerivedType(typeof(PublicListElementDto), typeDiscriminator: "List")]
    [JsonDerivedType(typeof(PublicSelectElementDto), typeDiscriminator: "Select")]
    public record class PublicElementDto
    {
        [JsonPropertyName("type")]
        public required string Type { get; init; }

        [JsonPropertyName("id")]
        public required string Id { get; init; }

        [JsonPropertyName("name")]
        public string? Name { get; init; }

        [JsonPropertyName("content")]
        public string? Content { get; init; }

        [JsonPropertyName("styles")]
        public Dictionary<string, object> Styles { get; init; } = [];

        [JsonPropertyName("tailwindStyles")]
        public string? TailwindStyles { get; init; }

        [JsonPropertyName("x")]
        public double X { get; init; }

        [JsonPropertyName("y")]
        public double Y { get; init; }

        [JsonPropertyName("src")]
        public string? Src { get; init; }

        [JsonPropertyName("href")]
        public string? Href { get; init; }
    }

    public record class PublicTextElementDto : PublicElementDto
    {
    }

    public record class PublicImageElementDto : PublicElementDto
    {
    }

    public record class PublicLinkElementDto : PublicElementDto
    {
    }

    public record class PublicButtonElementDto : PublicElementDto
    {
    }

    public record class PublicCarouselElementDto : PublicElementDto
    {
        [JsonPropertyName("elements")]
        public List<PublicElementDto> Elements { get; init; } = [];
        [JsonPropertyName("carouselSettings")]
        public Dictionary<string, object>? CarouselSettings { get; init; }
    }

    public record class PublicFrameElementDto : PublicElementDto
    {
        [JsonPropertyName("elements")]
        public List<PublicElementDto> Elements { get; init; } = [];
    }

    public record class PublicInputElementDto : PublicElementDto
    {
        [JsonPropertyName("inputSettings")]
        public Dictionary<string, object> InputSettings { get; init; } = [];
    }

    public record class PublicListElementDto : PublicElementDto
    {
        [JsonPropertyName("elements")]
        public List<PublicElementDto> Elements { get; init; } = [];
    }

    public record class PublicSelectElementDto : PublicElementDto
    {
        [JsonPropertyName("options")]
        public List<Dictionary<string, object>> Options { get; init; } = [];
    }
}