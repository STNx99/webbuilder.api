using webbuilder.api.dtos.imagedtos;

namespace webbuilder.api.services.interfaces
{
    public interface IImageService
    {
        Task<List<ImageDto>> GetAllImagesAsync();
        Task<bool> AddImagesAsync(List<ImageDto> images);
        Task<bool> DeleteImageAsync(string imageId);
    }
}