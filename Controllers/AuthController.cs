using Api.Data;
using Api.Dto;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
	public class AuthController : Controller
	{
		private readonly AppDBContext _context;
		private readonly JwtService _jwtService;

		public AuthController(AppDBContext context, JwtService jwtService)
		{
			_context = context;
			_jwtService = jwtService;
		}

		[HttpPost("login")]
		public async Task<ActionResult> Login(LoginDto loginDto)
		{
			// Verificar si el usuario existe en la base de datos
			var usuario = await _context.Usuarios
				.FirstOrDefaultAsync(u => u.NombreUsuario == loginDto.Usuario && u.Password == loginDto.Password);

			if (usuario == null)
			{
				return Unauthorized(new { mensaje = "Nombre de usuario o contraseña incorrectos." });
			}

			// Generar el JWT usando el rol del usuario
			var rol = usuario.RolID == 1 ? "Administrador" : "Colaborador";
			var token = _jwtService.GenerateToken(usuario, rol);

			return Ok(new { token });
		}

	}
}
