using ChannelEngine.Assessment.Acl;
using System.Threading;
using System.Threading.Tasks;

namespace ChannelEngine.Assessment.Application.Services
{
    public interface IWarehouseApplicationService
    {
        Task<bool> UpdateProductQuantityAsync(string productId, int quantity, CancellationToken cancellationToken = default);
    }

    public class WarehouseApplicationService : IWarehouseApplicationService
    {
        private readonly IProductAdapter _productAdapter;

        public WarehouseApplicationService(IProductAdapter productAdapter)
        {
            _productAdapter = productAdapter;
        }

        public async Task<bool> UpdateProductQuantityAsync(string productId, int quantity, CancellationToken cancellationToken = default)
        {
            return await _productAdapter.UpdateProductQuantityAsync(productId, quantity, cancellationToken);
        }
    }
}
