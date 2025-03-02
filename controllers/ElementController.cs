using Microsoft.AspNetCore.Mvc;
using webbuilder.api.data;
using webbuilder.api.dtos;
using webbuilder.api.mapping;
using webbuilder.api.models;

namespace webbuilder.api.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ElementController : ControllerBase
    {
        private readonly ElementStoreContext dbContext;

        public ElementController(ElementStoreContext context)
        {
            dbContext = context;
        }

        [HttpPost]
        public IActionResult Post(ElementDto element)
        {
            Element newElement = element.ToElement();
            dbContext.Elements.Add(newElement);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}