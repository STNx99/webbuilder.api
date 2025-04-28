using webbuilder.api.dtos;
using webbuilder.api.dtos.projectdtos;
using webbuilder.api.mapping;
using webbuilder.api.repositories.interfaces;

namespace webbuilder.api.services
{
    public class ProjectsService : IProjectsService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProjectsService(IProjectRepository projectRepository, IHttpContextAccessor httpContextAccessor)
        {
            _projectRepository = projectRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        // Helper method to get current user ID from HttpContext
        private string GetCurrentUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null || !httpContext.Items.TryGetValue("userId", out var userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }
            return userId.ToString();
        }

        public async Task<ProjectDto> CreateProjectAsync(ProjectDto project)
        {
            ArgumentNullException.ThrowIfNull(project);

            string userIdString = GetCurrentUserId();

            var projectExists = await _projectRepository.ExistsByNameAsync(project.Name, userIdString);
            if (projectExists)
            {
                throw new ArgumentException("Project with this name already exists");
            }

            var newProject = project.ToProject(userIdString);
            var createdProject = await _projectRepository.CreateAsync(newProject);
            return createdProject.ToProjectDto();
        }

        public async Task<IEnumerable<ProjectDto>> GetProjectsAsync()
        {
            try
            {
                string userId = GetCurrentUserId();
                var projects = await _projectRepository.GetByUserIdAsync(userId);
                return projects.Select(p => p.ToProjectDto());
            }
            catch (UnauthorizedAccessException)
            {
                return [];
            }
        }

        public async Task<bool> DeleteProjectAsync(string id)
        {
            try
            {
                string userId = GetCurrentUserId();
                return await _projectRepository.DeleteAsync(id, userId);
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
        public async Task<ProjectDto?> GetProjectByIdAsync(string id)
        {
            string userIdString = GetCurrentUserId();
            var project = await _projectRepository.GetByIdAsync(id, userIdString);
            return project?.ToProjectDto();
        }
        public async Task<bool> UpdateProjectAsync(string id, UpdateProjectDto projectDto)
        {
            ArgumentNullException.ThrowIfNull(projectDto);
            string userIdString = GetCurrentUserId();
            if (userIdString == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }
            var result = await _projectRepository.UpdateAsync(id, projectDto);
            return result;
        }
    }
}