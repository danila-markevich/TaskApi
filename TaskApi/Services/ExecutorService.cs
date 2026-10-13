using Microsoft.EntityFrameworkCore;
using TaskApi.Data;
using TaskApi.DTOs;
using TaskApi.Exceptions;
using TaskApi.Models;

namespace TaskApi.Services
{
    public class ExecutorService : IExecutorService
    {
        private readonly ApplicationDbContext _context;

        public ExecutorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExecutorDto>> GetAllAsync()
        {
            var executors = await _context.Executors
                .Select(e => new ExecutorDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    Specialty = e.Specialty,
                    TasksCount = e.Tasks.Count(),
                })
                .ToListAsync();

            return executors;
        }

        public async Task<ExecutorDto> GetByIdAsync(int id)
        {
            var executor = await _context.Executors
                .Select(e => new ExecutorDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,   
                    Specialty = e.Specialty,
                    TasksCount = e.Tasks.Count(),
                })
                .FirstOrDefaultAsync(e => e.Id == id);

            if (executor == null)
                throw new NotFoundException($"Executor with id {id} not found");

            return executor;
        }

        public async Task<ExecutorDto> CreateAsync(CreateExecutorDto dto)
        {
            var executor = new Executor
            {
                Name = dto.Name,
                Email = dto.Email,
                Specialty = dto.Specialty,
            };

            _context.Executors.Add(executor);
            await _context.SaveChangesAsync();

            var result = new ExecutorDto
            {
                Id = executor.Id,
                Name = executor.Name,
                Email = executor.Email,
                Specialty= executor.Specialty,
                TasksCount = 0
            };

            return result;
        }

        public async Task UpdateAsync(int id, UpdateExecutorDto dto)
        {
            var existing = await _context.Executors.FindAsync(id);
            if (existing == null)
                throw new NotFoundException($"Executor with id {id} not found");

            existing.Name = dto.Name;
            existing.Email = dto.Email;
            existing.Specialty = dto.Specialty;

            await _context.SaveChangesAsync();


        }

        public async Task DeleteAsync(int id)
        {
            var executor = await _context.Executors
                .Include(e => e.Tasks)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (executor == null)
                throw new NotFoundException($"Executor with id {id} not found");

            _context.Executors.Remove(executor);
            await _context.SaveChangesAsync();

        }
    }
}

