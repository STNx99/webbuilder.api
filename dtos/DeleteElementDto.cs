using System.ComponentModel.DataAnnotations;

namespace webbuilder.api.dtos
{
    public record class DeleteElementDto
    {
        [Required] public required string Id { get; init; }
    }
}