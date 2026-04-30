using CrmContactCenter.Server.DTOs.Accounts;
using CrmContactCenter.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmContactCenter.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>Obtener cuentas de un cliente</summary>
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomer(int customerId)
        {
            var accounts = await _accountService.GetByCustomerAsync(customerId);
            return Ok(accounts);
        }

        /// <summary>Obtener cuenta por ID</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var account = await _accountService.GetByIdAsync(id);
            if (account == null) return NotFound(new { message = "Cuenta no encontrada" });
            return Ok(account);
        }

        /// <summary>Crear nueva cuenta/deuda</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountDto dto)
        {
            var account = await _accountService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = account.Id }, account);
        }

        /// <summary>Actualizar cuenta</summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAccountDto dto)
        {
            var account = await _accountService.UpdateAsync(id, dto);
            if (account == null) return NotFound(new { message = "Cuenta no encontrada" });
            return Ok(account);
        }

        /// <summary>Eliminar cuenta</summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _accountService.DeleteAsync(id);
            if (!result) return NotFound(new { message = "Cuenta no encontrada" });
            return Ok(new { message = "Cuenta eliminada correctamente" });
        }
    }
}
