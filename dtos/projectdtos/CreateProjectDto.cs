using System.ComponentModel.DataAnnotations;

namespace webbuilder.api.dtos
{
    public class ProjectDto
    {
        [Required] public required string Name { get; set; }
        public string? Description { get; set; }
    }
}