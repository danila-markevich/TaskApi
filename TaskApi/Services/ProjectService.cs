using Microsoft.EntityFrameworkCore;
using TaskApi.Data;
using TaskApi.DTOs;
using TaskApi.Exceptions;
using TaskApi.Models;

namespace TaskApi.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        
        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProjectDto>> GetAllAsync()
        {
            var projects = await _context.Projects
                .Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    TasksCount = p.Tasks.Count(),
                })
                .ToListAsync();

            return projects;
        }

        public async Task<ProjectDto> GetByIdAsync (int id)
        {
            var project = await _context.Projects
                .Select (p => new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description= p.Description,
                    StartDate= p.StartDate,
                    TasksCount= p.Tasks.Count(),
                })
                .FirstOrDefaultAsync(p=>  p.Id == id);

            if (project == null)
                throw new NotFoundException($"Project with id {id} not found");

            return project;
        }

        public async Task<ProjectDto> CreateAsync(CreateProjectDto dto)
        {
            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                StartDate = DateTime.Now
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            var result = new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                TasksCount = 0
            };

            return result;
        }

        public async Task UpdateAsync(int id, UpdateProjectDto dto)
        {
            var existing = await _context.Projects.FindAsync(id);
            if (existing == null)
                throw new NotFoundException($"Project with id {id} not found");

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.StartDate = dto.StartDate;

            await _context.SaveChangesAsync();


        }

        public async Task DeleteAsync(int id)
        {
            var project = await _context.Projects
                .Include(p=> p.Tasks)
                .FirstOrDefaultAsync(p=>  p.Id == id);

            if (project == null)
                throw new NotFoundException($"Project with id {id} not found");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

        }
    }
}
