using webbuilder.api.dtos.projectdtos;
using webbuilder.api.models;

namespace webbuilder.api.repositories.interfaces
{
    public interface IProjectRepository
    {
        Task<Project> CreateAsync(Project project);
        Task<IEnumerable<Project>> GetByUserIdAsync(string userId);
        Task<Project?> GetByIdAsync(string id, string userId);
        Task<bool> DeleteAsync(string id, string userId);
        Task<bool> ExistsByNameAsync(string name, string userId);
        Task<bool> UpdateAsync(string id, UpdateProjectDto project);
    }
}