using CrmContactCenter.Server.DTOs.Users;

namespace CrmContactCenter.Server.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto> CreateAsync(CreateUserDto dto);
        Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto);
        Task<bool> ToggleActiveAsync(int id);
    }
}
