using CrmContactCenter.Server.Data;
using CrmContactCenter.Server.DTOs.Interactions;
using CrmContactCenter.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using CrmContactCenter.Server.Models.Enums;
using CrmContactCenter.Server.Models;

namespace CrmContactCenter.Server.Services
{
    public class InteractionService: IInteractionService
    {
        private readonly CrmDbContext _context;

        public InteractionService(CrmDbContext context)
        {
            _context = context;
        }

        public async Task<List<InteractionDto>> GetAllAsync(int? customerId, int? agentId, string? channel, DateTime? from, DateTime? to)
        {
            var query = _context.Interactions
                .Include(i => i.Customer)
                .Include(i => i.Agent)
                .AsQueryable();

            if (customerId.HasValue)
                query = query.Where(i => i.CustomerId == customerId);

            if (agentId.HasValue)
                query = query.Where(i => i.AgentId == agentId);

            if (!string.IsNullOrWhiteSpace(channel))
                query = query.Where(i => i.Channel.ToString() == channel);

            if (from.HasValue)
                query = query.Where(i => i.InteractionDate >= from);

            if (to.HasValue)
                query = query.Where(i => i.InteractionDate <= to);

            return await query
                .OrderByDescending(i => i.InteractionDate)
                .Select(i => ToDto(i))
                .ToListAsync();
        }

        public async Task<List<InteractionDto>> GetByCustomerAsync(int customerId)
        {
            return await _context.Interactions
                .Include(i => i.Customer)
                .Include(i => i.Agent)
                .Where(i => i.CustomerId == customerId)
                .OrderByDescending(i => i.InteractionDate)
                .Select(i => ToDto(i))
                .ToListAsync();
        }

        public async Task<InteractionDto?> GetByIdAsync(int id)
        {
            var interaction = await _context.Interactions
                .Include(i => i.Customer)
                .Include(i => i.Agent)
                .FirstOrDefaultAsync(i => i.Id == id);

            return interaction == null ? null : ToDto(interaction);
        }

        public async Task<InteractionDto> CreateAsync(CreateInteractionDto dto)
        {
            var interaction = new Interaction
            {
                CustomerId = dto.CustomerId,
                AgentId = dto.AgentId,
                AccountId = dto.AccountId,
                Channel = Enum.Parse<InteractionChannel>(dto.Channel),
                Result = Enum.Parse<InteractionResult>(dto.Result),
                Notes = dto.Notes,
                DurationSeconds = dto.DurationSeconds,
                InteractionDate = DateTime.Now,
                CreatedAt = DateTime.Now
            };

            _context.Interactions.Add(interaction);
            await _context.SaveChangesAsync();

            await _context.Entry(interaction).Reference(i => i.Customer).LoadAsync();
            await _context.Entry(interaction).Reference(i => i.Agent).LoadAsync();
            return ToDto(interaction);
        }

        private static InteractionDto ToDto(Interaction i) => new InteractionDto
        {
            Id = i.Id,
            CustomerId = i.CustomerId,
            CustomerName = i.Customer != null ? $"{i.Customer.FirstName} {i.Customer.LastName}" : string.Empty,
            AgentId = i.AgentId,
            AgentName = i.Agent != null ? $"{i.Agent.FirstName} {i.Agent.LastName}" : string.Empty,
            AccountId = i.AccountId,
            Channel = i.Channel.ToString(),
            Result = i.Result.ToString(),
            Notes = i.Notes,
            InteractionDate = i.InteractionDate,
            DurationSeconds = i.DurationSeconds ?? 0
        };
    }
}
