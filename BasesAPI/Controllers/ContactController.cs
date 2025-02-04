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
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly AppDbContext _context;


        public ContactController(IContactService contactService, AppDbContext context)
        {
            _contactService = contactService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddContact([FromQuery] int userId, [FromBody] ContactDto contactDto)
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

            // Proceder con la adhesión del nuevo contacto
            var result = await _contactService.AddContactAsync(userId, contactDto);

            return CreatedAtAction(nameof(GetContactById), new { userId, contactId = result.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts([FromQuery] int userId)
        {
            var result = await _contactService.GetContactsAsync(userId);
            return Ok(result);
        }

        [HttpGet("{contactId}")]
        public async Task<IActionResult> GetContactById(int userId, int contactId)
        {
            var result = await _contactService.GetContactByIdAsync(userId, contactId);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{contactId}")]
        public async Task<IActionResult> DeleteContact(int userId, int contactId)
        {
            var result = await _contactService.DeleteContactAsync(userId, contactId);
            return result ? NoContent() : NotFound();
        }
    }

}
