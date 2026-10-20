using AutoMapper;
using Azure.Core;
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

        public async Task<PagedResultDto<ExecutorDto>> GetAllAsync(PagedRequestDto request)
        {
            var query = _context.Executors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(e => e.Name.Contains(request.Search));
            }

            query = request.SortBy?.ToLower() switch
            {
                "name" => request.SortDesc
                    ? query.OrderByDescending(e => e.Name)
                    : query.OrderBy(e => e.Name),
                "email" => request.SortDesc
                    ? query.OrderByDescending(e => e.Email)
                    : query.OrderBy(e => e.Email),
                "specialty" => request.SortDesc
                    ? query.OrderByDescending(e => e.Specialty)
                    : query.OrderBy(e => e.Specialty),
                _ => query.OrderBy(e => e.Id)
            };

            var totalCount = await query.CountAsync();

            var executors = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Include(e => e.Tasks)
                .ToListAsync();

            var items = _mapper.Map<List<ExecutorDto>>(executors);

            return new PagedResultDto<ExecutorDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
            };
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