using CrmContactCenter.Server.DTOs.FollowUps;
using CrmContactCenter.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmContactCenter.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FollowUpsController : ControllerBase
    {
        private readonly IFollowUpService _followUpService;

        public FollowUpsController(IFollowUpService followUpService)
        {
            _followUpService = followUpService;
        }

        /// <summary>Listar follow-ups con filtros opcionales</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? agentId,
            [FromQuery] string? status)
        {
            try
            {
                var followUps = await _followUpService.GetAllAsync(agentId, status);
                return Ok(followUps);
            }
            catch (ArgumentException ex) when (ex.ParamName is "status")
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>Obtener follow-ups próximos (7 días)</summary>
        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcoming()
        {
            var followUps = await _followUpService.GetUpcomingAsync();
            return Ok(followUps);
        }

        /// <summary>Obtener follow-up por ID</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var followUp = await _followUpService.GetByIdAsync(id);
            if (followUp == null) return NotFound(new { message = "Seguimiento no encontrado" });
            return Ok(followUp);
        }

        /// <summary>Crear nuevo follow-up</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFollowUpDto dto)
        {
            var followUp = await _followUpService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = followUp.Id }, followUp);
        }

        /// <summary>Actualizar estado del follow-up</summary>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            try
            {
                var followUp = await _followUpService.UpdateStatusAsync(id, status);
                if (followUp == null) return NotFound(new { message = "Seguimiento no encontrado" });
                return Ok(followUp);
            }
            catch (ArgumentException ex) when (ex.ParamName is "status")
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
