using TaskApi.DTOs;

namespace TaskApi.Services
{
    public interface ITaskItemService
    {
        Task<PagedResultDto<TaskItemDto>> GetAllAsync(PagedRequestDto request);
        Task<TaskItemDto> GetByIdAsync(int id);
        Task<TaskItemDto> CreateAsync(CreateTaskItemDto dto);
        Task UpdateAsync(int id, UpdateTaskItemDto dto);
        Task DeleteAsync(int id);
        Task<List<TaskItemDto>> GetByProjectAsync(int  projectId);
        Task<List<TaskItemDto>> GetByExecutorAsync(int executorId);
        Task<List<TaskItemDto>> GetByStatusAsync(string status);
    }
}
