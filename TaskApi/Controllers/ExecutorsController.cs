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
    public class ExecutorsController : ControllerBase
    {
        private readonly IExecutorService _executorService;

        public ExecutorsController(IExecutorService executorService)
        {
            _executorService = executorService;

        }

        [HttpGet]
        public async Task<ActionResult<List<ExecutorDto>>> GetAll()
        {
            var result = await _executorService.GetAllAsync();
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExecutorDto>> GetById(int id)
        {
            var result = await _executorService.GetByIdAsync(id);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<ExecutorDto>> Create(CreateExecutorDto dto)
        {
            var result = await _executorService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateExecutorDto dto)
        {
            await _executorService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _executorService.DeleteAsync(id);
            return NoContent();
        }
    }
}
