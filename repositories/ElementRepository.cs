using Microsoft.EntityFrameworkCore;
using webbuilder.api.data;
using webbuilder.api.models;
using webbuilder.api.repositories.interfaces;

namespace webbuilder.api.repositories
{
    public class ElementRepository : IElementRepository
    {
        private readonly ElementStoreContext _dbContext;
        private Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction _transaction;

        public ElementRepository(ElementStoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Element> CreateAsync(Element element)
        {
            await _dbContext.Elements.AddAsync(element);
            await _dbContext.SaveChangesAsync();
            return element;
        }

        public async Task<IEnumerable<Element>> GetByProjectIdAsync(string projectId)
        {
            return await _dbContext.Elements
                .Where(e => e.ProjectId == projectId)
                .OrderBy(e => e.Order)
                .ToListAsync();
        }

        public async Task<Element?> GetByIdAsync(string id)
        {
            return await _dbContext.Elements
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> UpdateAsync(Element element)
        {
            _dbContext.Elements.Update(element);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var element = await GetByIdAsync(id);
            if (element == null) return false;

            _dbContext.Elements.Remove(element);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateOrdersAfterDeleteAsync(string? parentId, int order)
        {
            return await _dbContext.Elements
                .Where(e => e.ParentId == parentId && e.Order > order)
                .ExecuteUpdateAsync(e => e.SetProperty(x => x.Order, x => x.Order - 1)) > 0;
        }

        public async Task<int> GetChildCountAsync(string? parentId)
        {
            return await _dbContext.Elements
                .CountAsync(e => e.ParentId == parentId);
        }

        public async Task<List<string>> GetDescendantIdsAsync(string parentId)
        {
            var ids = new List<string>();
            await CollectDescendantIds(parentId, ids);
            return ids;
        }

        private async Task CollectDescendantIds(string parentId, List<string> ids)
        {
            var childIds = await _dbContext.Elements
                .Where(e => e.ParentId == parentId)
                .Select(e => e.Id)
                .ToListAsync();

            foreach (var childId in childIds)
            {
                ids.Add(childId);
                await CollectDescendantIds(childId, ids);
            }
        }

        public async Task<bool> DeleteManyAsync(List<string> ids)
        {
            return await _dbContext.Elements
                .Where(e => ids.Contains(e.Id))
                .ExecuteDeleteAsync() > 0;
        }

        public async Task<IDisposable> BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
            return _transaction;
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }
    }
}