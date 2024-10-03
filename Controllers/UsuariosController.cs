using Api.Data;
using Api.Dto;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsuariosController : ControllerBase
	{

		private readonly AppDBContext _context;

		public UsuariosController(AppDBContext context)
		{
			_context = context;
		}

		[HttpPost("registrar")]
		[Authorize(Roles = "Administrador")]
		public async Task<ActionResult> Registrar(RegistrarDto registroDto)
		{
			// Verifica si el modelo es válido
			if (!ModelState.IsValid)
			{
				return BadRequest(new { mensaje = "Datos de registro inválidos." });
			}

			// Verifica que el rolid sea 1 o 2 "ADMINISTRADOR" o "COLABORADOR"
			if (!(registroDto.RolID == 1 || registroDto.RolID == 2))
			{
				return BadRequest(new { mensaje = "Rol inválido." });
			}

			// Verifica si el nombre de usuario ya está en uso
			var usuarioExistente = await _context.Usuarios
				.FirstOrDefaultAsync(u => u.NombreUsuario == registroDto.NombreUsuario);

			if (usuarioExistente != null)
			{
				return BadRequest(new { mensaje = "El nombre de usuario ya está en uso." });
			}

			// Crea un nuevo usuario
			Usuario usuario = new Usuario
			{
				NombreUsuario = registroDto.NombreUsuario,
				Password = registroDto.Password,
				RolID = registroDto.RolID
			};

			await _context.Usuarios.AddAsync(usuario);
			await _context.SaveChangesAsync();

			return Ok(new { mensaje = "Usuario registrado exitosamente." });
		}

	}
}
