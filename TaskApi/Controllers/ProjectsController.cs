using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using TaskApi.DTOs;
using TaskApi.Services;

namespace TaskApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        
        public ProjectsController (IProjectService projectService)
        {
            _projectService = projectService;

        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDto<ProjectDto>>> GetAll([FromQuery] PagedRequestDto request)
        {
            var result = await _projectService.GetAllAsync(request);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetById(int id)
        {
            var result = await _projectService.GetByIdAsync(id);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create (CreateProjectDto dto)
        {
            var result = await _projectService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update (int id, UpdateProjectDto dto)
        {
            await _projectService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (int id)
        {
            await _projectService.DeleteAsync(id);
            return NoContent();
        }
    }
}
