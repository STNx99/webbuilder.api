using webbuilder.api.dtos;

namespace webbuilder.api.services
{
    public interface IElementsService
    {
        Task<ElementDto> CreateElement(ElementDto element);
        Task<IEnumerable<ElementDto>> GetElements();

        Task<bool> DeleteElement(DeleteElementDto element)

        Task<bool> UpdateElement(UpdateElementDto element);
    }
}