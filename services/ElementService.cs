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

            var newElement = element.ToElement(childCount);
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
            var elementToDelete = await _dbContext.Elements
                .Include(e => (e as FrameElement).Elements)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (elementToDelete == null)
            {
                return false;
            }

            var siblings = await _dbContext.Elements
                .Where(e => e.ParentId == elementToDelete.ParentId && e.Order > elementToDelete.Order)
                .ToListAsync();

            foreach (var sibling in siblings)
            {
                sibling.Order--;
            }

            if (elementToDelete is FrameElement frameElementToDelete && frameElementToDelete.Elements != null)
            {
                foreach (var child in frameElementToDelete.Elements.ToList())
                {
                    await DeleteElement(child.Id);
                }
            }

            _dbContext.Elements.Remove(elementToDelete);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BatchCreateElements(IEnumerable<CreateElementDto> elements)
        {
            if (!elements.Any())
            {
                throw new ArgumentException("The elements collection cannot be empty.");
            }

            var parentId = elements.First().ParentId;

            var childCount = await _dbContext.Elements
                .CountAsync(e => e.ParentId == parentId);

            foreach (var element in elements)
            {
                var newElement = element.ToElement(childCount);
                await _dbContext.Elements.AddAsync(newElement);
                childCount++;
            }

            await _dbContext.SaveChangesAsync();
            return true;
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