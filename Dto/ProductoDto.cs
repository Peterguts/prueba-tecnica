using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Dto
{
	public class ProductoDto
	{
		public int ProductoID { get; set; }

		public string Nombre { get; set; } = string.Empty;

		public string? Descripcion { get; set; }

		public decimal Precio { get; set; }

		public string SKU { get; set; } = string.Empty;

		public int Inventario { get; set; }

		public string? ImagenURL { get; set; }
	}
}
