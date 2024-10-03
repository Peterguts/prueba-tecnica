using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
	public class Rol
	{
		[Key]
		public int RolID { get; set; }

		[Required]
		[MaxLength(50)]
		public string NombreRol { get; set; }
	}
}
