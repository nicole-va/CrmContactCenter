using CrmContactCenter.Server.DTOs.Interactions;

namespace CrmContactCenter.Server.Services.Interfaces
{
    public interface IInteractionService
    {
        Task<List<InteractionDto>> GetAllAsync(int? customerId, int? agentId, string? channel, DateTime? from, DateTime? to);
        Task<List<InteractionDto>> GetByCustomerAsync(int customerId);
        Task<InteractionDto?> GetByIdAsync(int id);
        Task<InteractionDto> CreateAsync(CreateInteractionDto dto);
    }
}
