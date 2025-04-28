using webbuilder.api.dtos;
using webbuilder.api.dtos.projectdtos;
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
                Subdomain = project.Subdomain,
                Published = project.Published,
                OwnerId = ownerId
            };
        }

        public static Project ToProject(this UpdateProjectDto project, string projectId, string ownerId)
        {
            return new Project
            {
                Id = projectId,
                Name = project.Name ?? string.Empty,
                Description = project.Description ?? string.Empty,
                Subdomain = project.Subdomain,
                Published = project.Published,
                OwnerId = ownerId
            };
        }

        public static ProjectDto ToProjectDto(this Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Subdomain = project.Subdomain,
                Published = project.Published,
                Styles = new Dictionary<string, object>() // Initialize empty styles or map from project if available
            };
        }
    }
}