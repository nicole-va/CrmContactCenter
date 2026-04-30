using CrmContactCenter.Server.Data;
using CrmContactCenter.Server.DTOs.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using CrmContactCenter.Server.Services.Interfaces;

namespace CrmContactCenter.Server.Services
{
    public class AuthService: IAuthService
    {
        private readonly CrmDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(CrmDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            // Buscar usuario por email incluyendo su rol
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.IsActive == true);

            if (user == null) return null;

            // Verificar contraseña con BCrypt
            bool validPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!validPassword) return null;

            // Actualizar último login
            user.LastLogin = DateTime.Now;
            await _context.SaveChangesAsync();

            // Generar token JWT
            var token = GenerateToken(user.Id, user.Email, user.Role.Name);
            var hours = int.Parse(_config["JwtSettings:ExpirationHours"]!);
            var expiresAt = DateTime.Now.AddHours(hours);

            return new LoginResponseDto
            {
                Token = token,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Role = user.Role.Name,
                ExpiresAt = expiresAt
            };
        }

        private string GenerateToken(int userId, string email, string role)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secret = jwtSettings["SecretKey"];
            if (string.IsNullOrWhiteSpace(secret))
                throw new InvalidOperationException("JwtSettings:SecretKey no está configurado. Debe tener al menos 32 caracteres (256 bits) para HS256.");

            var secretBytes = Encoding.UTF8.GetBytes(secret);
            if (secretBytes.Length < 32)
                throw new InvalidOperationException($"JwtSettings:SecretKey es demasiado corto para HS256. Mínimo 32 bytes (256 bits). Actual: {secretBytes.Length} bytes.");

            var key = new SymmetricSecurityKey(secretBytes);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var hours = int.Parse(jwtSettings["ExpirationHours"]!);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email,          email),
                new Claim(ClaimTypes.Role,           role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(hours),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

