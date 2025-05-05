using Microsoft.AspNetCore.Mvc;
using webbuilder.api.dtos;
using webbuilder.api.dtos.projectdtos;
using webbuilder.api.services;

namespace webbuilder.api.controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsService _projectsService;

        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProjectDto project)
        {
            var result = await _projectsService.CreateProjectAsync(project);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _projectsService.GetProjectsAsync();
            return Ok(result);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _projectsService.DeleteProjectAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _projectsService.GetProjectByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateProjectDto project)
        {
            var result = await _projectsService.UpdateProjectAsync(id, project);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}