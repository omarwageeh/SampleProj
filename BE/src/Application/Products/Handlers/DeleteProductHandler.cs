using Application.Common;
using Application.Interfaces;
using Application.Products.Commands;

namespace Application.Products.Handlers
{
    public class DeleteProductHandler : ICommandHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _repository;
        public DeleteProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> HandleAsync(DeleteProductCommand command)
        {
            return await _repository.DeleteAsync(command.Id);
        }
    }
} 