using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
	public class Producto
	{
		[Key]
		public int ProductoID { get; set; }

		[Required]
		[MaxLength(100)]
		public string Nombre { get; set; } = string.Empty;

		[MaxLength(500)]
		public string Descripcion { get; set; } = string.Empty;

		[Required]
		[Column(TypeName = "decimal(10, 2)")]
		public decimal Precio { get; set; }

		[Required]
		[MaxLength(50)]
		public string SKU { get; set; } = string.Empty;

		[Required]
		public int Inventario { get; set; }

		public string ImagenURL { get; set; } = string.Empty;
	}
}
