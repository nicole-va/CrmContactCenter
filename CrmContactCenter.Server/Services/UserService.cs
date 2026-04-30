using CrmContactCenter.Server.Data;
using CrmContactCenter.Server.DTOs.Users;
using CrmContactCenter.Server.Models;
using CrmContactCenter.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace CrmContactCenter.Server.Services
{
    public class UserService: IUserService
    {
        private readonly CrmDbContext _context;

        public UserService(CrmDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Select(u => ToDto(u))
                .ToListAsync();
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            return user == null ? null : ToDto(user);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            var user = new User
            {
                RoleId = dto.RoleId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Phone = dto.Phone,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await _context.Entry(user).Reference(u => u.Role).LoadAsync();
            return ToDto(user);
        }

        public async Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return null;

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Phone = dto.Phone;
            user.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return ToDto(user);
        }

        public async Task<bool> ToggleActiveAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.IsActive = !user.IsActive;
            user.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        private static UserDto ToDto(User u) => new UserDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            Phone = u.Phone,
            IsActive = u.IsActive ?? true,
            Role = u.Role?.Name ?? string.Empty,
            CreatedAt = u.CreatedAt
        };
    }
}

