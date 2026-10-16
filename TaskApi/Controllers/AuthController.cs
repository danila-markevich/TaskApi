using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskApi.DTOs;
using TaskApi.Services;

namespace TaskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController (IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return result;
        }

        [HttpPost("login")]
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return result;
        }
    }
}
