using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
	public class Usuario
	{
		[Key]
		public int UsuarioID { get; set; }

		[Required]
		[MaxLength(50)]
		public string NombreUsuario { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		public int RolID { get; set; }

		public Rol Rol { get; set; }
	}
}
