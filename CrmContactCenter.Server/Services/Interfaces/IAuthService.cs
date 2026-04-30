namespace CrmContactCenter.Server.Services.Interfaces
{
    using CrmContactCenter.Server.DTOs.Auth;

    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);

    }
}
