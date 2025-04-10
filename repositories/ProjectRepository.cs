using Microsoft.EntityFrameworkCore;
using webbuilder.api.data;
using webbuilder.api.models;
using webbuilder.api.repositories.interfaces;

namespace webbuilder.api.repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ElementStoreContext _dbContext;

        public ProjectRepository(ElementStoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Project> CreateAsync(Project project)
        {
            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync();
            return project;
        }

        public async Task<IEnumerable<Project>> GetByUserIdAsync(string userId)
        {
            return await _dbContext.Projects
                .Where(p => p.OwnerId == userId)
                .ToListAsync();
        }

        public async Task<Project?> GetByIdAsync(string id, string userId)
        {
            return await _dbContext.Projects
                .Include(p => p.Elements)
                .FirstOrDefaultAsync(p => p.Id == id && p.OwnerId == userId);
        }

        public async Task<bool> DeleteAsync(string id, string userId)
        {
            var project = await _dbContext.Projects
                .Include(p => p.Elements)
                .FirstOrDefaultAsync(p => p.Id == id && p.OwnerId == userId);

            if (project == null)
            {
                return false;
            }

            _dbContext.Projects.Remove(project);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsByNameAsync(string name, string userId)
        {
            return await _dbContext.Projects
                .AnyAsync(p => p.Name == name && p.OwnerId == userId);
        }
    }
}