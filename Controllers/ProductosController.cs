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
	public class ProductosController : ControllerBase
	{
		private readonly AppDBContext _context;

		public ProductosController(AppDBContext context)
		{
			_context = context;
		}

		// GET: api/productos
		[HttpGet("disponibles")]
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

		// GET: api/productos
		[HttpGet("todos"), Authorize(Roles = "Administrador, Colaborador")]
		public async Task<ActionResult<IEnumerable<Producto>>> GetProductosAll()
		{
			List<ProductoDto> productos = await _context.Productos
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
		public async Task<ActionResult<Producto>> GetProducto(int id)
		{
			var producto = await _context.Productos.FindAsync(id);

			if (producto == null)
			{
				return NotFound();
			}

			var productoDto = new ProductoDto
			{
				ProductoID = producto.ProductoID,
				Nombre = producto.Nombre,
				Descripcion = producto.Descripcion,
				Precio = producto.Precio,
				SKU = producto.SKU,
				Inventario = producto.Inventario,
				ImagenURL = producto.ImagenURL
			};

			return Ok(productoDto);
		}

		// POST: api/productos
		[HttpPost, Authorize(Roles = "Administrador, Colaborador")]
		public async Task<ActionResult<Producto>> PostProducto(Producto producto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_context.Productos.Add(producto);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetProducto), new { id = producto.ProductoID }, producto);
		}

		// PUT: api/productos/{id}
		[HttpPut("{id}"), Authorize(Roles = "Administrador, Colaborador")]
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
		[HttpDelete("{id}"), Authorize(Roles = "Administrador")]
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
