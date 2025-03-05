using webbuilder.api.data;
using webbuilder.api.dtos;

namespace webbuilder.api.services
{
    public interface IProjectsService
    {
        public Task<bool> CreateProjectAsync(ProjectDto project);
        public Task<IEnumerable<ProjectDto>> GetProjectsAsync();
        public Task<bool> DeleteProjectAsync(string id);
    }
}