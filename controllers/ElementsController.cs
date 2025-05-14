using Microsoft.AspNetCore.Mvc;
using webbuilder.api.dtos;
using webbuilder.api.services;
using webbuilder.api.mapping;

namespace webbuilder.api.controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ElementsController : ControllerBase
    {
        private readonly IElementsService _elementsService;

        public ElementsController(IElementsService elementsService)
        {
            _elementsService = elementsService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateElementDto element)
        {
            var result = await _elementsService.CreateElement(element);
            return Ok(result);
        }

        [HttpPost("batch")]
        public async Task<IActionResult> BatchPost([FromBody] BatchCreateElementsDto request)
        {
            var result = await _elementsService.BatchCreateElements(request.Elements);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _elementsService.GetElements(id);
            return Ok(result);
        }

        [HttpGet("public/{id}")]
        public async Task<IActionResult> GetPublicElement(string id)
        {
            var result = await _elementsService.GetElements(id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _elementsService.DeleteElement(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateElementDto element)
        {
            var result = await _elementsService.UpdateElement(element);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}