using System.ComponentModel.DataAnnotations;

namespace Api.Dto
{
	public class RegistrarDto
	{
		[Required]
		public string NombreUsuario { get; set; } = string.Empty;	

		[Required]
		public string Password { get; set; } = string.Empty;	

		[Required]
		public int RolID { get; set; }
	}
}
