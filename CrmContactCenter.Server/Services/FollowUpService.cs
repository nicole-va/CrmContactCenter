using CrmContactCenter.Server.Data;
using CrmContactCenter.Server.DTOs.FollowUps;
using CrmContactCenter.Server.Models;
using CrmContactCenter.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using CrmContactCenter.Server.Models.Enums;


namespace CrmContactCenter.Server.Services
{
    public class FollowUpService: IFollowUpService
    {
        private readonly CrmDbContext _context;

        public FollowUpService(CrmDbContext context)
        {
            _context = context;
        }

        public async Task<List<FollowUpDto>> GetAllAsync(int? agentId, string? status)
        {
            var query = _context.FollowUps
                .Include(f => f.Customer)
                .Include(f => f.Agent)
                .AsQueryable();

            if (agentId.HasValue)
                query = query.Where(f => f.AgentId == agentId);

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (!Enum.TryParse<FollowUpStatus>(status, ignoreCase: true, out var parsedStatus))
                    throw new ArgumentException($"Status inválido: '{status}'.", nameof(status));

                query = query.Where(f => f.Status == parsedStatus);
            }

            return await query
                .OrderBy(f => f.ScheduledDate)
                .Select(f => ToDto(f))
                .ToListAsync();
        }

        public async Task<List<FollowUpDto>> GetUpcomingAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var in7days = today.AddDays(7);

            return await _context.FollowUps
                .Include(f => f.Customer)
                .Include(f => f.Agent)
                .Where(f => f.Status == FollowUpStatus.pendiente
                         && f.ScheduledDate >= today
                         && f.ScheduledDate <= in7days)
                .OrderBy(f => f.ScheduledDate)
                .Select(f => ToDto(f))
                .ToListAsync();
        }

        public async Task<FollowUpDto?> GetByIdAsync(int id)
        {
            var followUp = await _context.FollowUps
                .Include(f => f.Customer)
                .Include(f => f.Agent)
                .FirstOrDefaultAsync(f => f.Id == id);

            return followUp == null ? null : ToDto(followUp);
        }

        public async Task<FollowUpDto> CreateAsync(CreateFollowUpDto dto)
        {
            var followUp = new FollowUp
            {
                CustomerId = dto.CustomerId,
                AgentId = dto.AgentId,
                InteractionId = dto.InteractionId,
                ScheduledDate = dto.ScheduledDate,
                Notes = dto.Notes,
                Status = FollowUpStatus.pendiente,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.FollowUps.Add(followUp);
            await _context.SaveChangesAsync();

            await _context.Entry(followUp).Reference(f => f.Customer).LoadAsync();
            await _context.Entry(followUp).Reference(f => f.Agent).LoadAsync();
            return ToDto(followUp);
        }

        public async Task<FollowUpDto?> UpdateStatusAsync(int id, string status)
        {
            var followUp = await _context.FollowUps
                .Include(f => f.Customer)
                .Include(f => f.Agent)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (followUp == null) return null;

            if (!Enum.TryParse<FollowUpStatus>(status, ignoreCase: true, out var parsedStatus))
                throw new ArgumentException($"Status inválido: '{status}'.", nameof(status));

            followUp.Status = parsedStatus;
            followUp.UpdatedAt = DateTime.Now;

            if (followUp.Status == FollowUpStatus.completado)
                followUp.CompletedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return ToDto(followUp);
        }

        private static FollowUpDto ToDto(FollowUp f) => new FollowUpDto
        {
            Id = f.Id,
            CustomerId = f.CustomerId,
            CustomerName = f.Customer != null ? $"{f.Customer.FirstName} {f.Customer.LastName}" : string.Empty,
            CustomerPhone = f.Customer?.Phone ?? string.Empty,
            AgentId = f.AgentId,
            AgentName = f.Agent != null ? $"{f.Agent.FirstName} {f.Agent.LastName}" : string.Empty,
            InteractionId = f.InteractionId,
            ScheduledDate = f.ScheduledDate,
            Notes = f.Notes,
            Status = f.Status.ToString(),
            CompletedAt = f.CompletedAt,
            DaysUntil = f.ScheduledDate.DayNumber - DateOnly.FromDateTime(DateTime.Today).DayNumber
        };
    }
}
