using Moq;
using Application.Products.Handlers;
using Application.Products.Commands;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Tests
{
	public class CreateProductHandlerTests
	{
		[Fact]
		public async Task HandleAsync_ShouldAddProductAndReturnDto()
		{
			// Arrange
			var mockRepo = new Mock<IProductRepository>();
			var handler = new CreateProductHandler(mockRepo.Object);
			var command = new CreateProductCommand
			{
				Name = "UnitTest Product",
				Description = "UnitTest Desc",
				Price = 42.5M
			};
			var product = new Product { Id = 1, Name = command.Name, Description = command.Description, Price = command.Price };
			mockRepo.Setup(r => r.AddAsync(It.IsAny<Product>())).ReturnsAsync(product);

			// Act
			var result = await handler.HandleAsync(command);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(product.Id, result.Id);
			Assert.Equal(product.Name, result.Name);
			Assert.Equal(product.Description, result.Description);
			Assert.Equal(product.Price, result.Price);
			mockRepo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
		}
	}
}