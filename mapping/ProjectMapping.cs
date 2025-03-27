using webbuilder.api.dtos;
using webbuilder.api.models;

namespace webbuilder.api.mapping
{
    public static class ProjectMapping
    {
        public static Project ToProject(this ProjectDto project, string ownerId)
        {
            return new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = project.Name,
                Description = project.Description,
                OwnerId = ownerId
            };
        }

        public static ProjectDto ToProjectDto(this Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description
            };
        }
    }
}