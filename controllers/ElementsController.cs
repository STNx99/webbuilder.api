using Microsoft.AspNetCore.Mvc;
using webbuilder.api.dtos;
using webbuilder.api.services;

namespace webbuilder.api.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ElementsController : ControllerBase
    {
        private readonly IElementsService _elementsService;

        public ElementsController(IElementsService elementsService)
        {
            _elementsService = elementsService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ElementDto element)
        {
            var result = await _elementsService.CreateElement(element);
            return Ok(result);
        }

        public async Task<IActionResult> Get()
        {
            var result = await _elementsService.GetElements();
            return Ok(result);
        }
    }
}