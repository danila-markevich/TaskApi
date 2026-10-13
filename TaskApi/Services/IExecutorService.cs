using TaskApi.DTOs;

namespace TaskApi.Services
{
    public interface IExecutorService
    {
        Task<List<ExecutorDto>> GetAllAsync();
        Task<ExecutorDto> GetByIdAsync(int id);
        Task<ExecutorDto> CreateAsync(CreateExecutorDto dto);
        Task UpdateAsync(int id, UpdateExecutorDto dto);
        Task DeleteAsync(int id);
    }
}
