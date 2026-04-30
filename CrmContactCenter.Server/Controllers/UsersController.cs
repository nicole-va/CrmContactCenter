using CrmContactCenter.Server.DTOs.Users;
using CrmContactCenter.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmContactCenter.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>Obtener todos los usuarios</summary>
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        /// <summary>Obtener usuario por ID</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound(new { message = "Usuario no encontrado" });
            return Ok(user);
        }

        /// <summary>Crear nuevo usuario</summary>
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            var user = await _userService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        /// <summary>Actualizar usuario</summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            var user = await _userService.UpdateAsync(id, dto);
            if (user == null) return NotFound(new { message = "Usuario no encontrado" });
            return Ok(user);
        }

        /// <summary>Activar o desactivar usuario</summary>
        [HttpPatch("{id}/toggle-active")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var result = await _userService.ToggleActiveAsync(id);
            if (!result) return NotFound(new { message = "Usuario no encontrado" });
            return Ok(new { message = "Estado actualizado correctamente" });
        }
    }
}
