using Microsoft.AspNetCore.Mvc;
using Application.Products.Commands;
using Application.Products.Queries;
using Application.Products.Dtos;
using Application.Common;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace SampleApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductsController : ControllerBase
	{
		private readonly ICommandHandler<CreateProductCommand, ProductDto> _createHandler;
		private readonly ICommandHandler<UpdateProductCommand, bool> _updateHandler;
		private readonly ICommandHandler<DeleteProductCommand, bool> _deleteHandler;
		private readonly IQueryHandler<GetProductsQuery, IEnumerable<ProductDto>> _getAllHandler;
		private readonly IQueryHandler<GetProductByIdQuery, ProductDto?> _getByIdHandler;

		public ProductsController(
			ICommandHandler<CreateProductCommand, ProductDto> createHandler,
			ICommandHandler<UpdateProductCommand, bool> updateHandler,
			ICommandHandler<DeleteProductCommand, bool> deleteHandler,
			IQueryHandler<GetProductsQuery, IEnumerable<ProductDto>> getAllHandler,
			IQueryHandler<GetProductByIdQuery, ProductDto?> getByIdHandler)
		{
			_createHandler = createHandler;
			_updateHandler = updateHandler;
			_deleteHandler = deleteHandler;
			_getAllHandler = getAllHandler;
			_getByIdHandler = getByIdHandler;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
		{
			var result = await _getAllHandler.HandleAsync(new GetProductsQuery());
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ProductDto>> GetProduct(int id)
		{
			var result = await _getByIdHandler.HandleAsync(new GetProductByIdQuery { Id = id });
			if (result == null) return NotFound();
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<ProductDto>> CreateProduct([FromForm] CreateProductCommand command, IFormFile? image)
		{
			if (image != null && image.Length > 0)
			{
				var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
				if (!Directory.Exists(imagesPath))
					Directory.CreateDirectory(imagesPath);
				var fileName = $"{Guid.NewGuid()}_{image.FileName}";
				var filePath = Path.Combine(imagesPath, fileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await image.CopyToAsync(stream);
				}
				command.ImageUrl = $"/images/{fileName}";
			}
			var result = await _createHandler.HandleAsync(command);
			return CreatedAtAction(nameof(GetProduct), new { id = result.Id }, result);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateProduct(int id, [FromForm] UpdateProductCommand command, IFormFile? image)
		{
			if (id != command.Id) return BadRequest();
			if (image != null && image.Length > 0)
			{
				var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
				if (!Directory.Exists(imagesPath))
					Directory.CreateDirectory(imagesPath);
				var fileName = $"{Guid.NewGuid()}_{image.FileName}";
				var filePath = Path.Combine(imagesPath, fileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await image.CopyToAsync(stream);
				}
				command.ImageUrl = $"/images/{fileName}";
			}
			var updated = await _updateHandler.HandleAsync(command);
			if (!updated) return NotFound();
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var deleted = await _deleteHandler.HandleAsync(new DeleteProductCommand { Id = id });
			if (!deleted) return NotFound();
			return NoContent();
		}
	}
}