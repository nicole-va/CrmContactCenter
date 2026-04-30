using CrmContactCenter.Server.DTOs.Auth;
using CrmContactCenter.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrmContactCenter.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>Login de usuario</summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);
            if (result == null)
                return Unauthorized(new { message = "Credenciales incorrectas" });

            return Ok(result);
        }

        /// <summary>Generar hash de contraseña (solo desarrollo)</summary>
        [HttpGet("hash/{password}")]
        public IActionResult GenerateHash(string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            return Ok(new { hash });
        }
    }
}
