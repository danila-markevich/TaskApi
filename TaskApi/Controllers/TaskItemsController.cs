using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskApi.DTOs;
using TaskApi.Services;

namespace TaskApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {

        private readonly ITaskItemService _taskItemService;

        public TaskItemsController (ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskItemDto>>> GetAll()
        {
            var result = await   _taskItemService.GetAllAsync();
            return result;
        }

        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<List<TaskItemDto>>> GetByProject (int projectId)
        {
            var result = await _taskItemService.GetByProjectAsync(projectId);
            return result;
        }

        [HttpGet("executor/{executorId}")]
        public async Task<ActionResult<List<TaskItemDto>>> GetByExecutor (int executorId)
        {
            var result = await _taskItemService.GetByExecutorAsync(executorId);
            return result;
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<List<TaskItemDto>>> GetByStatus (string status)
        {
            var result = await _taskItemService.GetByStatusAsync(status);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItemDto>> GetById (int id)
        {
            var result = await _taskItemService.GetByIdAsync(id);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<TaskItemDto>> Create(CreateTaskItemDto dto)
        {
            var result = await _taskItemService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update (int id, UpdateTaskItemDto dto)
        {
            await _taskItemService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (int id)
        {
            await _taskItemService.DeleteAsync(id);
            return NoContent();
        }
    }
}
