using CrmContactCenter.Server.DTOs.Customers;
using CrmContactCenter.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmContactCenter.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>Listar clientes con búsqueda opcional por nombre, cédula o teléfono</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            var customers = await _customerService.GetAllAsync(search);
            return Ok(customers);
        }

        /// <summary>Obtener cliente por ID</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null) return NotFound(new { message = "Cliente no encontrado" });
            return Ok(customer);
        }

        /// <summary>Crear nuevo cliente</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
        {
            var customer = await _customerService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        /// <summary>Actualizar cliente</summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerDto dto)
        {
            var customer = await _customerService.UpdateAsync(id, dto);
            if (customer == null) return NotFound(new { message = "Cliente no encontrado" });
            return Ok(customer);
        }

        /// <summary>Eliminar cliente</summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _customerService.DeleteAsync(id);
            if (!result) return NotFound(new { message = "Cliente no encontrado" });
            return Ok(new { message = "Cliente eliminado correctamente" });
        }
    }
}
