using AutoMapper;
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
        private readonly IMapper _mapper;

        public ExecutorService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ExecutorDto>> GetAllAsync()
        {
            var executors = await _context.Executors
                .Include(e => e.Tasks)
                .ToListAsync();

            return _mapper.Map<List<ExecutorDto>>(executors);
        }

        public async Task<ExecutorDto> GetByIdAsync(int id)
        {
            var executor = await _context.Executors
                .Include(e => e.Tasks)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (executor == null)
                throw new NotFoundException($"Executor with id {id} not found");

            return _mapper.Map<ExecutorDto>(executor);
        }

        public async Task<ExecutorDto> CreateAsync(CreateExecutorDto dto)
        {
            var executor = _mapper.Map<Executor>(dto);

            _context.Executors.Add(executor);
            await _context.SaveChangesAsync();

            return _mapper.Map<ExecutorDto>(executor);
        }

        public async Task UpdateAsync(int id, UpdateExecutorDto dto)
        {
            var existing = await _context.Executors.FindAsync(id);
            if (existing == null)
                throw new NotFoundException($"Executor with id {id} not found");

            _mapper.Map(dto, existing);

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