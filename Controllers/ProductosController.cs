using Api.Data;
using Api.Dto;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Administrador, Colaborador")]
	public class ProductosController : ControllerBase
	{
		private readonly AppDBContext _context;

		public ProductosController(AppDBContext context)
		{
			_context = context;
		}

		// GET: api/productos
		[HttpGet]
		//[Authorize(Policy = "AdminOnly, UserPolicy")]
		[Authorize(Roles = "Administrador, Colaborador")]
		public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
		{
			List<ProductoDto> productos = await _context.Productos
			.Where(p => p.Inventario > 5)
			.Select(p => new ProductoDto
			{
				ProductoID = p.ProductoID,
				Nombre = p.Nombre,
				Descripcion = p.Descripcion,
				Precio = p.Precio,
				SKU = p.SKU,
				Inventario = p.Inventario,
				ImagenURL = p.ImagenURL
			})
			.ToListAsync();

			return Ok(productos);
		}

		// GET: api/productos/{id}
		[HttpGet("{id}")]
		[Authorize(Policy = "AdminOnly, UserPolicy")]
		public async Task<ActionResult<Producto>> GetProducto(int id)
		{
			var producto = await _context.Productos.FindAsync(id);

			if (producto == null)
			{
				return NotFound();
			}

			return producto;
		}

		// POST: api/productos
		[HttpPost]
		[Authorize(Policy = "AdminOnly, UserPolicy")]  // Solo Administrador y Colaborador pueden crear
		public async Task<ActionResult<Producto>> PostProducto(Producto producto)
		{
			_context.Productos.Add(producto);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetProducto), new { id = producto.ProductoID }, producto);
		}

		// PUT: api/productos/{id}
		[HttpPut("{id}")]
		[Authorize(Policy = "AdminOnly, UserPolicy")]
		public async Task<IActionResult> PutProducto(int id, Producto producto)
		{
			if (id != producto.ProductoID)
			{
				return BadRequest();
			}

			_context.Entry(producto).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ProductoExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// DELETE: api/productos/{id}
		[HttpDelete("{id}")]
		[Authorize(Policy = "AdminOnly")]  // Solo Administrador puede eliminar
		public async Task<IActionResult> DeleteProducto(int id)
		{
			var producto = await _context.Productos.FindAsync(id);
			if (producto == null)
			{
				return NotFound();
			}

			_context.Productos.Remove(producto);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool ProductoExists(int id)
		{
			return _context.Productos.Any(e => e.ProductoID == id);
		}
	}
}
