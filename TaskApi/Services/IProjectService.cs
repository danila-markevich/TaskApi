using TaskApi.DTOs;

namespace TaskApi.Services
{
    public interface IProjectService
    {
        Task<List<ProjectDto>> GetAllAsync();
        Task<ProjectDto> GetByIdAsync(int id);
        Task<ProjectDto> CreateAsync(CreateProjectDto dto);
        Task UpdateAsync(int id, UpdateProjectDto dto);
        Task DeleteAsync(int id);
    }
}
