using webbuilder.api.dtos;
using webbuilder.api.mapping;
using webbuilder.api.models;
using webbuilder.api.repositories.interfaces;
using webbuilder.api.services.interfaces;

namespace webbuilder.api.services
{
    public class ElementsService : IElementsService
    {
        private readonly IElementRepository _elementRepository;
        private readonly ISettingsService _settingsService;

        public ElementsService(IElementRepository elementRepository, ISettingsService settingsService)
        {
            _elementRepository = elementRepository;
            _settingsService = settingsService;
        }

        public async Task<ElementDto> CreateElement(CreateElementDto element)
        {
            using (await _elementRepository.BeginTransactionAsync())
            {
                try
                {
                    var childCount = await _elementRepository.GetChildCountAsync(element.ParentId);

                    var newElement = element.ToElement(childCount);
                    var createdElement = await _elementRepository.CreateAsync(newElement);

                    if (element.Type == "Input" && element is CreateElementDto { InputSettings: not null } inputElement)
                    {
                        await _settingsService.UpdateElementSettingAsync(createdElement.Id, "input", inputElement.InputSettings);
                    }
                    else if (element.Type == "Select" && element is CreateElementDto { SelectSettings: not null } selectElement)
                    {
                        await _settingsService.UpdateElementSettingAsync(createdElement.Id, "select", selectElement.SelectSettings);
                    }
                    else if (element.Type == "Carousel" && element is CreateElementDto { CarouselSettings: not null } carouselElement)
                    {
                        await _settingsService.UpdateElementSettingAsync(createdElement.Id, "carousel", carouselElement.CarouselSettings);
                    }

                    await _elementRepository.CommitTransactionAsync();
                    return createdElement.ToElementDto();
                }
                catch (Exception)
                {
                    await _elementRepository.RollbackTransactionAsync();
                    throw;
                }
            }
        }

        public async Task<IEnumerable<ElementDto>> GetElements(string id)
        {
            // Try to get elements by project ID first
            var elements = await _elementRepository.GetByProjectIdAsync(id);

            // If no elements found, the ID might be a parent ID of nested elements
            if (!elements.Any())
            {
                var element = await _elementRepository.GetByIdAsync(id);
                if (element != null)
                {
                    // Return empty collection for elements not found
                    return Enumerable.Empty<ElementDto>();
                }
            }

            var result = new List<ElementDto>();

            foreach (var element in elements.OrderBy(e => e.Order))
            {
                Dictionary<string, object>? settings = null;

                // Fetch specific settings based on element type
                if (element.Type == "Input")
                {
                    settings = await _settingsService.GetElementSettingAsync(element.Id, "input");
                }
                else if (element.Type == "Select")
                {
                    settings = await _settingsService.GetElementSettingAsync(element.Id, "select");
                }
                else if (element.Type == "Carousel")
                {
                    settings = await _settingsService.GetElementSettingAsync(element.Id, "carousel");
                }

                result.Add(element.ToElementDto(settings));
            }

            return result.Where(e=> e.ParentId == null);
        }

        public async Task<bool> DeleteElement(string id)
        {
            var allIds = new List<string> { id };
            var descendantIds = await _elementRepository.GetDescendantIdsAsync(id);
            allIds.AddRange(descendantIds);

            using (await _elementRepository.BeginTransactionAsync())
            {
                try
                {
                    var element = await _elementRepository.GetByIdAsync(id);
                    if (element == null) return false;

                    await _elementRepository.UpdateOrdersAfterDeleteAsync(element.ParentId, element.Order);

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
                            var createdElement = await _elementRepository.CreateAsync(newElement);

                            if (element.Type == "Input" && element is CreateElementDto { InputSettings: not null } inputElement)
                            {
                                await _settingsService.UpdateElementSettingAsync(createdElement.Id, "input", inputElement.InputSettings);
                            }
                            else if (element.Type == "Select" && element is CreateElementDto { SelectSettings: not null } selectElement)
                            {
                                await _settingsService.UpdateElementSettingAsync(createdElement.Id, "select", selectElement.SelectSettings);
                            }
                            else if (element.Type == "Carousel" && element is CreateElementDto { CarouselSettings: not null } carouselElement)
                            {
                                await _settingsService.UpdateElementSettingAsync(createdElement.Id, "carousel", carouselElement.CarouselSettings);
                            }
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
            try
            {
                var elementToUpdate = await _elementRepository.GetByIdAsync(element.Id);
                if (elementToUpdate == null)
                {
                    return false;
                }

                var elementType = element.Type;
                if (!string.IsNullOrEmpty(elementType))
                {
                    elementToUpdate.Type = elementType;
                }

                elementToUpdate.Name = element.Name;
                elementToUpdate.Content = element.Content;
                elementToUpdate.Styles = element.Styles;
                elementToUpdate.X = element.X;
                elementToUpdate.Y = element.Y;
                elementToUpdate.Src = element.Src ?? elementToUpdate.Src;
                elementToUpdate.Href = element.Href ?? elementToUpdate.Href;
                elementToUpdate.TailwindStyles = element.TailwindStyles;
                elementToUpdate.ParentId = element.ParentId;

                if (!string.IsNullOrEmpty(element.ProjectId) && elementToUpdate.ProjectId != element.ProjectId)
                {
                    elementToUpdate.ProjectId = element.ProjectId;
                }

                switch (elementType)
                {
                    case "Input":
                        if (element.InputSettings != null)
                        {
                            await _settingsService.UpdateElementSettingAsync(elementToUpdate.Id, "input", element.InputSettings);
                        }
                        break;
                    case "Select":
                        if (element.SelectSettings != null)
                        {
                            await _settingsService.UpdateElementSettingAsync(elementToUpdate.Id, "select", element.SelectSettings);
                        }
                        break;
                    case "Carousel":
                        if (element.CarouselSettings != null)
                        {
                            await _settingsService.UpdateElementSettingAsync(elementToUpdate.Id, "carousel", element.CarouselSettings);
                        }
                        break;
                    case "Frame":
                        break;
                    case "Button":
                        break;
                    default:
                        break;
                }

                return await _elementRepository.UpdateAsync(elementToUpdate);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error updating element: {ex.Message}");
                throw;
            }
        }
    }
}