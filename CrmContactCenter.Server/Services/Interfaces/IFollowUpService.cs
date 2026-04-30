using CrmContactCenter.Server.DTOs.FollowUps;

namespace CrmContactCenter.Server.Services.Interfaces
{
    public interface IFollowUpService
    {
        Task<List<FollowUpDto>> GetAllAsync(int? agentId, string? status);
        Task<List<FollowUpDto>> GetUpcomingAsync();
        Task<FollowUpDto?> GetByIdAsync(int id);
        Task<FollowUpDto> CreateAsync(CreateFollowUpDto dto);
        Task<FollowUpDto?> UpdateStatusAsync(int id, string status);
    }
}
