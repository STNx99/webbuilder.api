using webbuilder.api.dtos;

namespace webbuilder.api.services
{
    public interface IElementsService
    {
        Task<ElementDto> CreateElement(CreateElementDto element);
        Task<bool> BatchCreateElements(IEnumerable<CreateElementDto> elements);
        Task<IEnumerable<ElementDto>> GetElements(string id);
        Task<bool> DeleteElement(string id);
        Task<bool> UpdateElement(UpdateElementDto element);
    }
}