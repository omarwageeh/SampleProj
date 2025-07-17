using Application.Interfaces;
using Application.Products.Commands;
using Application.Products.Dtos;
using Application.Common;
using Domain.Entities;

namespace Application.Products.Handlers
{
    public class ProductCommandHandler :
        ICommandHandler<CreateProductCommand, ProductDto>,
        ICommandHandler<UpdateProductCommand, bool>,
        ICommandHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _repository;
        public ProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDto> HandleAsync(CreateProductCommand command)
        {
            var product = new Product
            {
                Name = command.Name,
                Description = command.Description,
                Price = command.Price
            };
            var created = await _repository.AddAsync(product);
            return new ProductDto
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description,
                Price = created.Price
            };
        }

        public async Task<bool> HandleAsync(UpdateProductCommand command)
        {
            var product = new Product
            {
                Id = command.Id,
                Name = command.Name,
                Description = command.Description,
                Price = command.Price
            };
            return await _repository.UpdateAsync(product);
        }

        public async Task<bool> HandleAsync(DeleteProductCommand command)
        {
            return await _repository.DeleteAsync(command.Id);
        }
    }
} 