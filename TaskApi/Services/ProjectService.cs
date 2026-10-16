using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProjectService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProjectDto>> GetAllAsync()
        {
            var projects = await _context.Projects
                .Include(p => p.Tasks)
                .ToListAsync();

            return _mapper.Map<List<ProjectDto>>(projects);
        }

        public async Task<ProjectDto> GetByIdAsync(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                throw new NotFoundException($"Project with id {id} not found");

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto> CreateAsync(CreateProjectDto dto)
        {
            var project = _mapper.Map<Project>(dto);

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task UpdateAsync(int id, UpdateProjectDto dto)
        {
            var existing = await _context.Projects.FindAsync(id);
            if (existing == null)
                throw new NotFoundException($"Project with id {id} not found");

            _mapper.Map(dto, existing);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                throw new NotFoundException($"Project with id {id} not found");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}