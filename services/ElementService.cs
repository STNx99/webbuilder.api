using Microsoft.EntityFrameworkCore;
using webbuilder.api.data;
using webbuilder.api.dtos;
using webbuilder.api.mapping;

namespace webbuilder.api.services
{
    public class ElementsService : IElementsService
    {
        private readonly ElementStoreContext _dbContext;

        public ElementsService(ElementStoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ElementDto> CreateElement(ElementDto element)
        {
            var newElement = element.ToElement();
            await _dbContext.Elements.AddAsync(newElement);
            await _dbContext.SaveChangesAsync();
            return newElement.ToElementDto();
        }

        public async Task<IEnumerable<ElementDto>> GetElements()
        {
            var elements = await _dbContext.Elements.ToListAsync();
            return elements.Select(e => e.ToElementDto()).ToList();
        }

        public async Task<bool> DeleteElement(DeleteElementDto element)
        {
            var elementToDelete = await _dbContext.Elements.FirstOrDefaultAsync(e => e.Id == element.Id);
            if (elementToDelete == null)
            {
                return false;
            }

            _dbContext.Elements.Remove(elementToDelete);
            await _dbContext.SaveChangesAsync();
            return true;
        }


    }
}