using webbuilder.api.data;
using webbuilder.api.models;
using webbuilder.api.repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace webbuilder.api.repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ElementStoreContext _context;

        public ImageRepository(ElementStoreContext context)
        {
            _context = context;
        }

        public async Task<List<Image>> GetAllImagesAsync(string userId)
        {
            return await _context.Images
                .Where(i => i.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> AddImagesAsync(List<Image> image)
        {
            if (image == null || image.Count == 0)
            {
                throw new ArgumentException("Image list cannot be null or empty");
            }

            await _context.Images.AddRangeAsync(image);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteImageAsync(string imageId)
        {
            var image = await _context.Images.FindAsync(imageId);
            if (image == null) return false;

            _context.Images.Remove(image);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}