using System.Text.Json.Serialization;

namespace webbuilder.api.dtos.imagedtos
{
    public record class ImageDto
    {
        [JsonPropertyName("imageLink")]
        public required string ImageLink { get; set; }

        [JsonPropertyName("imageName")]
        public string? ImageName { get; set; } = "";
    }
}