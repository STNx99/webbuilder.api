using System.ComponentModel.DataAnnotations;

namespace webbuilder.api.dtos
{
    public class ProjectDto
    {
        public string? Id { get; set; }
        [Required] public required string Name { get; set; }
        public string? Description { get; set; }
    }
}