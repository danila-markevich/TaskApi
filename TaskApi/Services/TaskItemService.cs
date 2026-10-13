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
        public TaskItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskItemDto>> GetAllAsync()
        {
            var tasks = await _context.TaskItems
                .Select(t => new TaskItemDto
                {
                    Id = t.Id,
                    ProjectId = t.ProjectId,
                    ProjectName = t.Project.Name != null ? t.Project.Name : null,
                    ExecutorId = t.ExecutorId,
                    ExecutorName = t.Executor.Name != null ? t.Executor.Name : null,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    Priority = t.Priority,
                    Deadline = t.Deadline,
                })
                .ToListAsync();

            return tasks;
        }

        public async Task<List<TaskItemDto>> GetByProjectAsync(int projectId)
        {
            var projectExist = await _context.Projects.AnyAsync(p=>p.Id == projectId);
            if (!projectExist)
                throw new NotFoundException($"Project with id {projectId} not found");

            var taskByProject = await _context.TaskItems
                .Where(t =>  t.ProjectId == projectId)
                .Select(t=> new TaskItemDto
                {
                    Id=t.Id,
                    ProjectId=t.ProjectId,
                    ProjectName = t.Project.Name != null ? t.Project.Name : null,
                    ExecutorId=t.ExecutorId,
                    ExecutorName = t.Executor.Name != null ? t.Executor.Name : null,
                    Title = t.Title,
                    Description =t.Description,
                    Status=t.Status,
                    Priority = t.Priority,
                    Deadline = t.Deadline,
                })
                .ToListAsync() ;

            return taskByProject;
        }

        public async Task<List<TaskItemDto>> GetByExecutorAsync(int executorId)
        {
            var executorExist = await _context.Executors.AnyAsync(e=> e.Id == executorId);
            if (!executorExist)
                throw new NotFoundException($"Executor with id {executorId} not found");
            var taskByExecutor = await _context.TaskItems
                .Where (t => t.ExecutorId == executorId)
                .Select(t=> new TaskItemDto
                {
                    Id = t.Id,
                    ProjectId = t.ProjectId,
                    ProjectName = t.Project.Name != null ? t.Project.Name : null,
                    ExecutorId = t.ExecutorId,
                    ExecutorName = t.Executor.Name != null ? t.Executor.Name : null,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    Priority = t.Priority,
                    Deadline = t.Deadline,
                })
                .ToListAsync();

            return taskByExecutor;
        }

        public async Task<List<TaskItemDto>> GetByStatusAsync(string status)
        {
            var taskByStatus = await _context.TaskItems
                .Where(t => t.Status == status)
                .Select (t=> new TaskItemDto
                {
                    Id = t.Id,
                    ProjectId = t.ProjectId,
                    ProjectName = t.Project.Name != null ? t.Project.Name : null,
                    ExecutorId = t.ExecutorId,
                    ExecutorName = t.Executor.Name != null ? t.Executor.Name : null,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    Priority = t.Priority,
                    Deadline = t.Deadline,
                })
                .ToListAsync (); 
            
            return taskByStatus;
        }




        public async Task<TaskItemDto> GetByIdAsync(int id)
        {
            var taskItem = await _context.TaskItems
                .Select (t => new TaskItemDto
                {
                    Id = t.Id,
                    ProjectId = t.ProjectId,
                    ProjectName = t.Project.Name != null ? t.Project.Name : null,
                    ExecutorId = t.ExecutorId,
                    ExecutorName = t.Executor.Name != null ? t.Executor.Name : null,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    Priority = t.Priority,
                    Deadline = t.Deadline,
                })
                .FirstOrDefaultAsync(t=>  t.Id == id);

            if (taskItem == null)
                throw new NotFoundException($"Task with id {id} not found");

            return taskItem;
        }


        public async Task<TaskItemDto> CreateAsync(CreateTaskItemDto dto)
        {
            var projectExist = await _context.Projects.FindAsync(dto.ProjectId);
            if (projectExist == null)
                throw new BadRequestException($"Project with id {dto.ProjectId} not found");

            var executorExist = await _context.Executors.FindAsync(dto.ExecutorId);
            if (executorExist == null)
                throw new BadRequestException($"Executor with id {dto.ExecutorId} not found");

            var taskItem = new TaskItem
            {
                ExecutorId = dto.ExecutorId,
                ProjectId = dto.ProjectId,
                Title = dto.Title,
                Description= dto.Description,
                Status= dto.Status,
                Priority    = dto.Priority,
                Deadline= dto.Deadline,
            };

            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();

            var result = new TaskItemDto
            {
                Id = taskItem.Id,
                ProjectId = taskItem.ProjectId,
                ProjectName = projectExist.Name,
                ExecutorId = taskItem.ExecutorId,
                ExecutorName = executorExist.Name,
                Title = taskItem.Title,
                Description = taskItem.Description,
                Status = taskItem.Status,
                Priority = taskItem.Priority,
                Deadline = taskItem.Deadline,
            };

            return result;
        }


        public async Task UpdateASync(int id, UpdateTaskItemDto dto)
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

            existing.ExecutorId = dto.ExecutorId;
            existing.ProjectId = dto.ProjectId;
            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.Status = dto.Status;
            existing.Priority = dto.Priority;
            existing.Deadline = dto.Deadline;

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
