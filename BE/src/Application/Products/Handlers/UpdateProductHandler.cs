using Application.Common;
using Application.Interfaces;
using Application.Products.Commands;
using Domain.Entities;

namespace Application.Products.Handlers
{
    public class UpdateProductHandler : ICommandHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _repository;
        public UpdateProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> HandleAsync(UpdateProductCommand command)
        {
            var product = new Product
            {
                Id = command.Id,
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                ImageUrl = command.ImageUrl
            };
            return await _repository.UpdateAsync(product);
        }
    }
} 