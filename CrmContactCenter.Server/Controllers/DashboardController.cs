using CrmContactCenter.Server.Data;
using CrmContactCenter.Server.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrmContactCenter.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly CrmDbContext _context;

        public DashboardController(CrmDbContext context)
        {
            _context = context;
        }

        /// <summary>Métricas generales del dashboard</summary>
        [HttpGet]
        public async Task<IActionResult> GetMetrics()
        {
            var today = DateTime.Today;

            var totalCustomers = await _context.Customers.CountAsync();

            var totalPending = await _context.Accounts
                .Where(a => a.Status == AccountStatus.pendiente)
                .SumAsync(a => a.Amount);

            var totalOverdue = await _context.Accounts
                .Where(a => a.Status == AccountStatus.vencido)
                .SumAsync(a => a.Amount);

            var interactionsToday = await _context.Interactions
                .Where(i => i.InteractionDate.Date == today)
                .CountAsync();

            var contacted = await _context.Interactions
                .Where(i => i.InteractionDate.Date == today && i.Result == InteractionResult.contactado)
                .CountAsync();

            var noAnswer = await _context.Interactions
                .Where(i => i.InteractionDate.Date == today && i.Result == InteractionResult.no_responde)
                .CountAsync();

            var promiseToPay = await _context.Interactions
                .Where(i => i.InteractionDate.Date == today && i.Result == InteractionResult.promesa_de_pago)
                .CountAsync();

            var paymentDone = await _context.Interactions
                .Where(i => i.InteractionDate.Date == today && i.Result == InteractionResult.pago_realizado)
                .CountAsync();

            var pendingFollowUps = await _context.FollowUps
                .Where(f => f.Status == FollowUpStatus.pendiente && f.ScheduledDate <= DateOnly.FromDateTime(today))
                .CountAsync();

            return Ok(new
            {
                totalCustomers,
                totalPending,
                totalOverdue,
                interactionsToday,
                contacted,
                noAnswer,
                promiseToPay,
                paymentDone,
                pendingFollowUps
            });
        }
    }
}
