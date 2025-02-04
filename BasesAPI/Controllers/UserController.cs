using BasesAPI.Models;
using BasesAPI.Models.DTOs;
using BasesAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BasesAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly AppDbContext _context;

        public UserController(IUserService userService, AppDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpPut("{userId}/email")]
        public async Task<IActionResult> ModifyEmail(int userId, [FromQuery] string email)
        {
            // Obtener identidad del usuario autenticado
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
            {
                return Unauthorized("Token inválido o inexistente.");
            }

            // Obtener el ID del usuario desde el token
            var userIdFromToken = identity.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userIdFromToken) || !int.TryParse(userIdFromToken, out var authenticatedUserId))
            {
                return Unauthorized("Token inválido o no contiene el ID de usuario.");
            }
            
            // Validar que el ID del usuario autenticado coincide con el proporcionado
            if (authenticatedUserId != userId)
            {
                return Forbid("El usuario autenticado no coincide con el ID proporcionado.");
            }

            // Verificar si el usuario existe en la base de datos
            var userExists = await _context.Users.AnyAsync(u => u.Id == authenticatedUserId);
            if (!userExists)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Proceder con la modificación del nuevo email
            var result = await _userService.ModifyEmail(userId, email);

            return Ok(result);

        }

        [HttpPut("modifypass")]
        public async Task<IActionResult> ModifyPass(int userId, [FromBody] ChangePass changePass)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
            {
                return Unauthorized("Token inválido o inexistente.");
            }

            var userIdFromToken = identity.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userIdFromToken) || !int.TryParse(userIdFromToken, out var authenticatedUserId))
            {
                return Unauthorized("Token inválido o no contiene el ID de usuario.");
            }

            if (authenticatedUserId != userId)
            {
                return Forbid("El usuario autenticado no coincide con el ID proporcionado.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == authenticatedUserId);
            if (!userExists)
            {
                return NotFound("Usuario no encontrado.");
            }

            var result = await _userService.ModifyPass(userId, changePass.CurrentPasswordHash, changePass.NewPasswordHash);

            return Ok("Registro exitoso.");
        }

    }
}
