using webbuilder.api.models;

namespace webbuilder.api.repositories.interfaces
{
    public interface IImageRepository
    {
        Task<List<Image>> GetAllImagesAsync(string userId);
        Task<bool> AddImagesAsync(List<Image> image);
        Task<bool> DeleteImageAsync(string imageId);
    }
}