using TaskApi.DTOs;

namespace TaskApi.Services
{
    public interface IProjectService
    {
        Task<PagedResultDto<ProjectDto>> GetAllAsync(PagedRequestDto request);
        Task<ProjectDto> GetByIdAsync(int id);
        Task<ProjectDto> CreateAsync(CreateProjectDto dto);
        Task UpdateAsync(int id, UpdateProjectDto dto);
        Task DeleteAsync(int id);
    }
}
