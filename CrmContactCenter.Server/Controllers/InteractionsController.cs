using CrmContactCenter.Server.DTOs.Interactions;
using CrmContactCenter.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmContactCenter.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InteractionsController : ControllerBase
    {
        private readonly IInteractionService _interactionService;

        public InteractionsController(IInteractionService interactionService)
        {
            _interactionService = interactionService;
        }

        /// <summary>Listar interacciones con filtros opcionales</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? customerId,
            [FromQuery] int? agentId,
            [FromQuery] string? channel,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            var interactions = await _interactionService.GetAllAsync(customerId, agentId, channel, from, to);
            return Ok(interactions);
        }

        /// <summary>Obtener interacciones de un cliente</summary>
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomer(int customerId)
        {
            var interactions = await _interactionService.GetByCustomerAsync(customerId);
            return Ok(interactions);
        }

        /// <summary>Obtener interacción por ID</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var interaction = await _interactionService.GetByIdAsync(id);
            if (interaction == null) return NotFound(new { message = "Interacción no encontrada" });
            return Ok(interaction);
        }

        /// <summary>Registrar nueva interacción</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInteractionDto dto)
        {
            var interaction = await _interactionService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = interaction.Id }, interaction);
        }

        /// <summary>Endpoint externo simulado (WhatsApp/Bot)</summary>
        [HttpPost("external")]
        [AllowAnonymous]
        public async Task<IActionResult> External([FromBody] CreateInteractionDto dto)
        {
            var interaction = await _interactionService.CreateAsync(dto);
            return Ok(new { message = "Interacción registrada desde canal externo", data = interaction });
        }
    }
}
