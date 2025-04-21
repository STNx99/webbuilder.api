using webbuilder.api.dtos;
using webbuilder.api.mapping;
using webbuilder.api.repositories.interfaces;

namespace webbuilder.api.services
{
    public class ElementsService : IElementsService
    {
        private readonly IElementRepository _elementRepository;

        public ElementsService(IElementRepository elementRepository)
        {
            _elementRepository = elementRepository;
        }

        public async Task<ElementDto> CreateElement(CreateElementDto element)
        {
            var childCount = await _elementRepository.GetChildCountAsync(element.ParentId);

            var newElement = element.ToElement(childCount);
            var createdElement = await _elementRepository.CreateAsync(newElement);
            return createdElement.ToElementDto();
        }

        public async Task<IEnumerable<ElementDto>> GetElements(string id)
        {
            var elements = await _elementRepository.GetByProjectIdAsync(id);

            return elements.Select(e => e.ToElementDto()).Where(e => e.ParentId == null);
        }

        public async Task<bool> DeleteElement(string id)
        {
            // Get all descendant IDs first
            var allIds = new List<string> { id };
            var descendantIds = await _elementRepository.GetDescendantIdsAsync(id);
            allIds.AddRange(descendantIds);

            using (await _elementRepository.BeginTransactionAsync())
            {
                try
                {
                    var element = await _elementRepository.GetByIdAsync(id);
                    if (element == null) return false;

                    // Update order of siblings
                    await _elementRepository.UpdateOrdersAfterDeleteAsync(element.ParentId, element.Order);

                    // Delete the element and all descendants
                    await _elementRepository.DeleteManyAsync(allIds);

                    await _elementRepository.CommitTransactionAsync();
                    return true;
                }
                catch
                {
                    await _elementRepository.RollbackTransactionAsync();
                    throw;
                }
            }
        }

        public async Task<bool> BatchCreateElements(IEnumerable<CreateElementDto> elements)
        {
            if (!elements.Any())
            {
                throw new ArgumentException("The elements collection cannot be empty.");
            }

            using (await _elementRepository.BeginTransactionAsync())
            {
                try
                {
                    var elementsByParent = elements.ToLookup(e => e.ParentId);

                    foreach (var parentIdGroup in elementsByParent)
                    {
                        var parentId = parentIdGroup.Key;

                        var childCount = await _elementRepository.GetChildCountAsync(parentId);

                        foreach (var element in parentIdGroup)
                        {
                            var newElement = element.ToElement(childCount);
                            await _elementRepository.CreateAsync(newElement);
                            childCount++;
                        }
                    }

                    await _elementRepository.CommitTransactionAsync();
                    return true;
                }
                catch (Exception)
                {
                    await _elementRepository.RollbackTransactionAsync();
                    throw;
                }
            }
        }

        public async Task<bool> UpdateElement(UpdateElementDto element)
        {
            var elementToUpdate = await _elementRepository.GetByIdAsync(element.Id);
            if (elementToUpdate == null)
            {
                return false;
            }

            elementToUpdate.Type = element.Type;
            elementToUpdate.Name = element.Name;
            elementToUpdate.Content = element.Content;
            elementToUpdate.Styles = element.Styles;
            elementToUpdate.X = element.X;
            elementToUpdate.Y = element.Y;
            elementToUpdate.Src = element.Src ?? elementToUpdate.Src;
            elementToUpdate.Href = element.Href ?? elementToUpdate.Href;
            elementToUpdate.TailwindStyles = element.TailwindStyles;
            elementToUpdate.ParentId = element.ParentId;
            elementToUpdate.ProjectId = element.ProjectId;
            return await _elementRepository.UpdateAsync(elementToUpdate);
        }
    }
}