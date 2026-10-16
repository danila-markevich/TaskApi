using AutoMapper;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskApi.Data;
using TaskApi.DTOs;
using TaskApi.Exceptions;
using TaskApi.Models;

namespace TaskApi.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TaskItemService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TaskItemDto>> GetAllAsync()
        {
            var tasks = await _context.TaskItems
                .Include(t => t.Project)
                .Include(t => t.Executor)
                .ToListAsync();

            return _mapper.Map<List<TaskItemDto>>(tasks);
        }

        public async Task<List<TaskItemDto>> GetByProjectAsync(int projectId)
        {
            var projectExist = await _context.Projects.AnyAsync(p => p.Id == projectId);
            if (!projectExist)
                throw new NotFoundException($"Project with id {projectId} not found");

            var tasks = await _context.TaskItems
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.Project)
                .Include(t => t.Executor)
                .ToListAsync();

            return _mapper.Map<List<TaskItemDto>>(tasks);
        }

        public async Task<List<TaskItemDto>> GetByExecutorAsync(int executorId)
        {
            var executorExist = await _context.Executors.AnyAsync(e => e.Id == executorId);
            if (!executorExist)
                throw new NotFoundException($"Executor with id {executorId} not found");

            var tasks = await _context.TaskItems
                .Where(t => t.ExecutorId == executorId)
                .Include(t => t.Project)
                .Include(t => t.Executor)
                .ToListAsync();

            return _mapper.Map<List<TaskItemDto>>(tasks);
        }

        public async Task<List<TaskItemDto>> GetByStatusAsync(string status)
        {
            var tasks = await _context.TaskItems
                .Where(t => t.Status == status)
                .Include(t => t.Project)
                .Include(t => t.Executor)
                .ToListAsync();

            return _mapper.Map<List<TaskItemDto>>(tasks);
        }

        public async Task<TaskItemDto> GetByIdAsync(int id)
        {
            var taskItem = await _context.TaskItems
                .Include(t => t.Project)
                .Include(t => t.Executor)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (taskItem == null)
                throw new NotFoundException($"Task with id {id} not found");

            return _mapper.Map<TaskItemDto>(taskItem);
        }

        public async Task<TaskItemDto> CreateAsync(CreateTaskItemDto dto)
        {
            var projectExist = await _context.Projects.FindAsync(dto.ProjectId);
            if (projectExist == null)
                throw new BadRequestException($"Project with id {dto.ProjectId} not found");

            var executorExist = await _context.Executors.FindAsync(dto.ExecutorId);
            if (executorExist == null)
                throw new BadRequestException($"Executor with id {dto.ExecutorId} not found");

            var taskItem = _mapper.Map<TaskItem>(dto);

            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();

            taskItem.Project = projectExist;
            taskItem.Executor = executorExist;

            return _mapper.Map<TaskItemDto>(taskItem);
        }

        public async Task UpdateAsync(int id, UpdateTaskItemDto dto)
        {
            var existing = await _context.TaskItems.FindAsync(id);
            if (existing == null)
                throw new NotFoundException($"Task with id {id} not found");

            if (dto.ExecutorId != existing.ExecutorId)
            {
                var executorExist = await _context.Executors.AnyAsync(e => e.Id == dto.ExecutorId);
                if (!executorExist)
                    throw new BadRequestException($"Executor with id {dto.ExecutorId} not found");
            }

            if (dto.ProjectId != existing.ProjectId)
            {
                var projectExist = await _context.Projects.AnyAsync(p => p.Id == dto.ProjectId);
                if (!projectExist)
                    throw new BadRequestException($"Project with id {dto.ProjectId} not found");
            }

            _mapper.Map(dto, existing);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
                throw new NotFoundException($"Task with id {id} not found");

            _context.TaskItems.Remove(taskItem);
            await _context.SaveChangesAsync();
        }
    }
}