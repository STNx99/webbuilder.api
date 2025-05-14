using webbuilder.api.dtos.imagedtos;
using webbuilder.api.models;
using webbuilder.api.repositories.interfaces;
using webbuilder.api.services.interfaces;
namespace webbuilder.api.services
{

    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly HttpContextAccessor _httpContextAccessor;

        private string GetCurrentUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null || !httpContext.Items.TryGetValue("userId", out var userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }
            return userId.ToString();
        }
        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
            _httpContextAccessor = new HttpContextAccessor();
        }

        public async Task<List<ImageDto>> GetAllImagesAsync()
        {
            var userId = GetCurrentUserId();
            var images = await _imageRepository.GetAllImagesAsync(userId);
            return images.Select(image => new ImageDto
            {
                ImageLink = image.ImageLink,
                ImageName = image.ImageName
            }).ToList();
        }

        public async Task<bool> AddImagesAsync(List<ImageDto> images)
        {
            if (images == null || images.Count == 0)
            {
                throw new ArgumentException("Image list cannot be null or empty");
            }
            var userId = GetCurrentUserId();
            var newImages = images.Select(imageDto => new Image
            {
                ImageId = Guid.NewGuid().ToString(),
                ImageLink = imageDto.ImageLink,
                ImageName = imageDto.ImageName,
                UserId = userId
            }).ToList();
            return await _imageRepository.AddImagesAsync(newImages);
        }

        public async Task<bool> DeleteImageAsync(string imageId)
        {
            return await _imageRepository.DeleteImageAsync(imageId);
        }
    }
}