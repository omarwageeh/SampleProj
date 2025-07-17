using Application.Common;
using Application.Interfaces;
using Application.Products.Dtos;
using Application.Products.Queries;

namespace Application.Products.Handlers
{
    public class GetProductByIdHandler : IQueryHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductRepository _repository;
        public GetProductByIdHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDto?> HandleAsync(GetProductByIdQuery query)
        {
            var product = await _repository.GetByIdAsync(query.Id);
            if (product == null) return null;
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl
            };
        }
    }
} 