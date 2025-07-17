using Application.Common;
using Application.Interfaces;
using Application.Products.Commands;
using Application.Products.Dtos;
using Domain.Entities;

namespace Application.Products.Handlers
{
    public class CreateProductHandler : ICommandHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _repository;
        public CreateProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDto> HandleAsync(CreateProductCommand command)
        {
            var product = new Product
            {
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                ImageUrl = command.ImageUrl
            };
            var created = await _repository.AddAsync(product);
            return new ProductDto
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description,
                Price = created.Price,
                ImageUrl = created.ImageUrl
            };
        }
    }
} 