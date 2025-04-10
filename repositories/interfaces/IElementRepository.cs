using webbuilder.api.dtos;
using webbuilder.api.models;

namespace webbuilder.api.repositories.interfaces
{
    public interface IElementRepository
    {
        Task<Element> CreateAsync(Element element);
        Task<IEnumerable<Element>> GetByProjectIdAsync(string projectId);
        Task<Element?> GetByIdAsync(string id);
        Task<bool> UpdateAsync(Element element);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateOrdersAfterDeleteAsync(string? parentId, int order);
        Task<int> GetChildCountAsync(string? parentId);
        Task<List<string>> GetDescendantIdsAsync(string parentId);
        Task<bool> DeleteManyAsync(List<string> ids);

        // Transaction-related methods
        Task<IDisposable> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}