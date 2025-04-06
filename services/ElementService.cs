using Microsoft.EntityFrameworkCore;
using webbuilder.api.data;
using webbuilder.api.dtos;
using webbuilder.api.mapping;
using webbuilder.api.models;

namespace webbuilder.api.services
{
    public class ElementsService : IElementsService
    {
        private readonly ElementStoreContext _dbContext;

        public ElementsService(ElementStoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ElementDto> CreateElement(CreateElementDto element)
        {
            var childCount = await _dbContext.Elements
                .CountAsync(e => e.ParentId == element.ParentId);

            var newElement = element.ToElement(childCount++);
            await _dbContext.Elements.AddAsync(newElement);
            await _dbContext.SaveChangesAsync();
            return newElement.ToElementDto();
        }

        public async Task<IEnumerable<ElementDto>> GetElements(string id)
        {
            var elements = await _dbContext.Elements
                .Where(e => e.ProjectId == id)
                .OrderBy(e => e.Order)
                .ToListAsync();

            return elements.Select(e => e.ToElementDto()).Where(e => e.ParentId == null);
        }
        public async Task<bool> DeleteElement(string id)
        {
            // Get all descendant IDs first
            var allIds = new List<string> { id };
            await GetDescendantIds(id, allIds);

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var element = await _dbContext.Elements
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (element == null) return false;

                await _dbContext.Elements
                    .Where(e => e.ParentId == element.ParentId && e.Order > element.Order)
                    .ExecuteUpdateAsync(e => e.SetProperty(x => x.Order, x => x.Order - 1));

                await _dbContext.Elements
                    .Where(e => allIds.Contains(e.Id))
                    .ExecuteDeleteAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task GetDescendantIds(string parentId, List<string> ids)
        {
            var childIds = await _dbContext.Elements
                .Where(e => e.ParentId == parentId)
                .Select(e => e.Id)
                .ToListAsync();

            foreach (var childId in childIds)
            {
                ids.Add(childId);
                await GetDescendantIds(childId, ids);
            }
        }

        public async Task<bool> BatchCreateElements(IEnumerable<CreateElementDto> elements)
        {
            if (!elements.Any())
            {
                throw new ArgumentException("The elements collection cannot be empty.");
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var elementsByParent = elements.ToLookup(e => e.ParentId);

                foreach (var parentIdGroup in elementsByParent)
                {
                    var parentId = parentIdGroup.Key;

                    var childCount = await _dbContext.Elements
                        .CountAsync(e => e.ParentId == parentId);

                    // Process all elements at this level
                    foreach (var element in parentIdGroup)
                    {
                        var newElement = element.ToElement(childCount);
                        await _dbContext.Elements.AddAsync(newElement);
                        childCount++;
                    }
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateElement(UpdateElementDto element)
        {
            var elementToUpdate = await _dbContext.Elements.FirstOrDefaultAsync(e => e.Id == element.Id);
            if (elementToUpdate == null)
            {
                return false;
            }

            elementToUpdate.Type = element.Type;
            elementToUpdate.Name = element.Name;
            elementToUpdate.Content = element.Content;
            elementToUpdate.IsSelected = element.IsSelected;
            elementToUpdate.Styles = element.Styles;
            elementToUpdate.X = element.X;
            elementToUpdate.Y = element.Y;
            elementToUpdate.Src = element.Src ?? elementToUpdate.Src;
            elementToUpdate.Href = element.Href ?? elementToUpdate.Href;

            await _dbContext.SaveChangesAsync();
            return true;
        }

    }
}