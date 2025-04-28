using webbuilder.api.dtos;
using webbuilder.api.dtos.projectdtos;

namespace webbuilder.api.services
{
    public interface IProjectsService
    {
        public Task<ProjectDto> CreateProjectAsync(ProjectDto project);
        public Task<IEnumerable<ProjectDto>> GetProjectsAsync();
        public Task<bool> DeleteProjectAsync(string id);
        public Task<ProjectDto?> GetProjectByIdAsync(string id);
        public Task<bool> UpdateProjectAsync(string id, UpdateProjectDto project);
    }
}