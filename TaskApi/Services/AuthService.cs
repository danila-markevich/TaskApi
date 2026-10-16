using Microsoft.EntityFrameworkCore;
using TaskApi.Data;
using TaskApi.DTOs;
using TaskApi.Exceptions;
using TaskApi.Models;

namespace TaskApi.Services
{
    public class AuthService : IAuthService
    {
        
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        
        public AuthService(
            ApplicationDbContext context,
            IPasswordHasher passwordHasher,
            ITokenService tokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            
            var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (exists)
                throw new BadRequestException($"User with email {dto.Email} already exists");

            
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = _passwordHasher.HashPassword(dto.Password),
                Role = "User"
            };

            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            
            var token = _tokenService.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                Role = user.Role
            };
        }

        
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                throw new BadRequestException("Invalid email or password");

            
            var passwordValid = _passwordHasher.VerifyPassword(dto.Password, user.PasswordHash);
            if (!passwordValid)
                throw new BadRequestException("Invalid email or password");

            
            var token = _tokenService.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}