using TaskApi.DTOs;

namespace TaskApi.Services
{
    public interface IExecutorService
    {
        Task<PagedResultDto<ExecutorDto>> GetAllAsync(PagedRequestDto request);
        Task<ExecutorDto> GetByIdAsync(int id);
        Task<ExecutorDto> CreateAsync(CreateExecutorDto dto);
        Task UpdateAsync(int id, UpdateExecutorDto dto);
        Task DeleteAsync(int id);
    }
}
