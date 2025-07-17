using Moq;
using Application.Products.Handlers;
using Application.Products.Queries;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Tests
{
	public class GetProductsHandlerTests
	{
		[Fact]
		public async Task HandleAsync_ShouldReturnAllProductsAsDtos()
		{
			// Arrange
			var mockRepo = new Mock<IProductRepository>();
			var handler = new GetProductsHandler(mockRepo.Object);
			var products = new List<Product>
		{
			new Product { Id = 1, Name = "A", Description = "D1", Price = 1 },
			new Product { Id = 2, Name = "B", Description = "D2", Price = 2 }
		};
			mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

			// Act
			var result = (await handler.HandleAsync(new GetProductsQuery())).ToList();

			// Assert
			Assert.Equal(products.Count, result.Count);
			Assert.Contains(result, p => p.Id == 1 && p.Name == "A");
			Assert.Contains(result, p => p.Id == 2 && p.Name == "B");
		}
	}
}