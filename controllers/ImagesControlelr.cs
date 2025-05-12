using Microsoft.AspNetCore.Mvc;
using webbuilder.api.dtos.imagedtos;
using webbuilder.api.services.interfaces;
namespace webbuilder.api.controllers
{

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _imageService.GetAllImagesAsync();
            return Ok(images);
        }

        [HttpPost]
        public async Task<IActionResult> AddImages([FromBody] List<ImageDto> image)
        {
            var result = await _imageService.AddImagesAsync(image);
            if (result)
                return Ok();
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(string id)
        {
            var result = await _imageService.DeleteImageAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}