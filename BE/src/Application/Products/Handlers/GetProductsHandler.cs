using Application.Common;
using Application.Interfaces;
using Application.Products.Dtos;
using Application.Products.Queries;

namespace Application.Products.Handlers
{
    public class GetProductsHandler : IQueryHandler<GetProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository _repository;
        public GetProductsHandler(IProductRepository repository)
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
                Price = p.Price,
                ImageUrl = p.ImageUrl
            });
        }
    }
} 