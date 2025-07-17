using Moq;
using Application.Products.Handlers;
using Application.Products.Commands;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Tests
{
	public class UpdateProductHandlerTests
	{
		[Fact]
		public async Task HandleAsync_ShouldUpdateProductAndReturnTrue()
		{
			// Arrange
			var mockRepo = new Mock<IProductRepository>();
			var handler = new UpdateProductHandler(mockRepo.Object);
			var command = new UpdateProductCommand
			{
				Id = 1,
				Name = "Updated Name",
				Description = "Updated Desc",
				Price = 100M
			};
			mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(true);

			// Act
			var result = await handler.HandleAsync(command);

			// Assert
			Assert.True(result);
			mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Once);
		}

		[Fact]
		public async Task HandleAsync_ShouldReturnFalseIfProductNotFound()
		{
			// Arrange
			var mockRepo = new Mock<IProductRepository>();
			var handler = new UpdateProductHandler(mockRepo.Object);
			var command = new UpdateProductCommand { Id = 99, Name = "X", Description = "Y", Price = 1 };
			mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(false);

			// Act
			var result = await handler.HandleAsync(command);

			// Assert
			Assert.False(result);
		}
	}
}