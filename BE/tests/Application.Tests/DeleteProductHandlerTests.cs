using Moq;
using Application.Products.Handlers;
using Application.Products.Commands;
using Application.Interfaces;

namespace Application.Tests
{
	public class DeleteProductHandlerTests
	{
		[Fact]
		public async Task HandleAsync_ShouldDeleteProductAndReturnTrue()
		{
			// Arrange
			var mockRepo = new Mock<IProductRepository>();
			var handler = new DeleteProductHandler(mockRepo.Object);
			var command = new DeleteProductCommand { Id = 1 };
			mockRepo.Setup(r => r.DeleteAsync(command.Id)).ReturnsAsync(true);

			// Act
			var result = await handler.HandleAsync(command);

			// Assert
			Assert.True(result);
			mockRepo.Verify(r => r.DeleteAsync(command.Id), Times.Once);
		}

		[Fact]
		public async Task HandleAsync_ShouldReturnFalseIfProductNotFound()
		{
			// Arrange
			var mockRepo = new Mock<IProductRepository>();
			var handler = new DeleteProductHandler(mockRepo.Object);
			var command = new DeleteProductCommand { Id = 99 };
			mockRepo.Setup(r => r.DeleteAsync(command.Id)).ReturnsAsync(false);

			// Act
			var result = await handler.HandleAsync(command);

			// Assert
			Assert.False(result);
		}
	}
}