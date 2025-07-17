using Moq;
using Application.Products.Handlers;
using Application.Products.Queries;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Tests
{
	public class GetProductByIdHandlerTests
	{
		[Fact]
		public async Task HandleAsync_ShouldReturnProductDtoIfFound()
		{
			// Arrange
			var mockRepo = new Mock<IProductRepository>();
			var handler = new GetProductByIdHandler(mockRepo.Object);
			var product = new Product { Id = 1, Name = "A", Description = "D", Price = 10 };
			mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

			// Act
			var result = await handler.HandleAsync(new GetProductByIdQuery { Id = 1 });

			// Assert
			Assert.NotNull(result);
			Assert.Equal(product.Id, result!.Id);
			Assert.Equal(product.Name, result.Name);
			Assert.Equal(product.Description, result.Description);
			Assert.Equal(product.Price, result.Price);
		}

		[Fact]
		public async Task HandleAsync_ShouldReturnNullIfNotFound()
		{
			// Arrange
			var mockRepo = new Mock<IProductRepository>();
			var handler = new GetProductByIdHandler(mockRepo.Object);
			mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Product?)null);

			// Act
			var result = await handler.HandleAsync(new GetProductByIdQuery { Id = 99 });

			// Assert
			Assert.Null(result);
		}
	}
}