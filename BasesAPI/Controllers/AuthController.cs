using BasesAPI.Models;
using BasesAPI.Models.DTOs;
using BasesAPI.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto userDto)
    {
        var user = new User { 
            Username = userDto.Username,
            PasswordHash = userDto.PasswordHash,
            Email = userDto.Email,
            BirthDate = userDto.BirthDate,
            Name = userDto.Name,
            LastName = userDto.LastName,
        };

        var result = await _userService.Register(user);

        if (!result)
            return BadRequest("Usuario ya registrado.");

        return Ok("Registro exitoso.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        // Llamar al servicio para validar el usuario
        var user = await _userService.Login(request.Username, request.Password);

        if (user == null)
            return Unauthorized("Usuario o contraseña incorrectos.");

        // Generar el token JWT pasando tanto userId como username
        var token = TokenService.GenerateToken(user.Id, user.Username);

        return Ok(new { Token = token });
    }

}