using Api.Data;
using Api.Dto;
using Api.Models;
using Api.Services;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
	public class AuthController : Controller
	{
		private readonly AppDBContext _context;
		private readonly JwtService _tokenService;

		public AuthController(AppDBContext context, JwtService tokenService)
		{
			_context = context;
			_tokenService = tokenService;
		}

		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginDto model)
		{
			// Verificar si el modelo es válido
			if (model == null || string.IsNullOrEmpty(model.Usuario) || string.IsNullOrEmpty(model.Password))
			{
				return BadRequest("Datos incompletos");
			}

			Usuario? user = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == model.Usuario);

			if (user == null)
			{
				return Unauthorized("Credenciales inválidas");
			}

			if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
			{
				return Unauthorized("Credenciales inválidas");
			}

			// Obtener el rol del usuario (con tipo explícito)
			string? role = _context.Roles.FirstOrDefault(r => r.RolID == user.RolID)?.NombreRol;

			if (role == null)
				return Unauthorized("Rol no válido");

			// Generar el token
			var token = _tokenService.GenerateToken(user, role);
			return Ok(token);
		}
	}
}
