using webbuilder.api.data;
using webbuilder.api.dtos;
using webbuilder.api.mapping;
using Microsoft.EntityFrameworkCore;

namespace webbuilder.api.services
{
    public class ProjectsService : IProjectsService
    {
        private readonly ElementStoreContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProjectsService(ElementStoreContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ProjectDto> CreateProjectAsync(ProjectDto project)
        {
            var find = await _context.Projects.FirstOrDefaultAsync(p => p.Name == project.Name);
            if (find != null)
            {
                throw new ArgumentException("Project with this name already exists");
            }
            ArgumentNullException.ThrowIfNull(project);

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return null;
            }

            if (!httpContext.Items.TryGetValue("userId", out var userId))
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            }

            var newProject = project.ToProject(userId.ToString());
            await _context.Projects.AddAsync(newProject);
            await _context.SaveChangesAsync();
            return newProject.ToProjectDto();
        }

        public async Task<IEnumerable<ProjectDto>> GetProjectsAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return Enumerable.Empty<ProjectDto>();
            }

            if (!httpContext.Items.TryGetValue("userId", out var userId))
            {
                return Enumerable.Empty<ProjectDto>();
            }

            var projects = await _context.Projects
                .Where(p => p.OwnerId == userId.ToString())
                .ToListAsync();

            return projects.Select(p => p.ToProjectDto());
        }

        public async Task<bool> DeleteProjectAsync(string id)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return false;
            }
            if (!httpContext.Items.TryGetValue("userId", out var userId))
            {
                return false;
            }
            var projectToDelete = await _context.Projects
                .Include(p => p.Elements)
                .FirstOrDefaultAsync(p => p.Id == id && p.OwnerId == userId.ToString());
            if (projectToDelete == null)
            {
                return false;
            }

            _context.Projects.Remove(projectToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}