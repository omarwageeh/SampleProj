using Application.Interfaces;
using Application.Products.Dtos;
using Application.Products.Queries;
using Application.Common;

namespace Application.Products.Handlers
{
    public class ProductQueryHandler :
        IQueryHandler<GetProductsQuery, IEnumerable<ProductDto>>,
        IQueryHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductRepository _repository;
        public ProductQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> HandleAsync(GetProductsQuery query)
        {
            var products = await _repository.GetAllAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price
            });
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
                Price = product.Price
            };
        }
    }
} 