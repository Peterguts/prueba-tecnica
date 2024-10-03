using System.ComponentModel.DataAnnotations;

namespace Api.Dto
{
	public class UsuarioDto
	{
		[Required]
		public string Usuario { get; set; } = string.Empty;
		[Required]
		public string Password { get; set; } = string.Empty;
	}
}
