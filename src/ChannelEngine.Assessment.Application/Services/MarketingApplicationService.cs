using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ChannelEngine.Assessment.Acl;
using ChannelEngine.Assessment.Application.DTOs;
using ChannelEngine.Assessment.Domain.Marketing.Models;
using ChannelEngine.Assessment.Domain.Marketing.Services;

namespace ChannelEngine.Assessment.Application.Services
{
    public interface IMarketingApplicationService
    {
        Task<List<BestSellerProductDto>> GetBestSellerProductsAsync(int count = 5, CancellationToken cancellationToken = default);
    }

    public class MarketingApplicationService : IMarketingApplicationService
    {
        private readonly IOrderAdapter _orderAdapter;
        private readonly IProductAdapter _productAdapter;
        private readonly IMarketingService _marketingService;

        public MarketingApplicationService(IOrderAdapter orderAdapter, IProductAdapter productAdapter, IMarketingService marketingService)
        {
            _orderAdapter = orderAdapter;
            _productAdapter = productAdapter;
            _marketingService = marketingService;
        }

        public async Task<List<BestSellerProductDto>> GetBestSellerProductsAsync(int count = 5, CancellationToken cancellationToken = default)
        {
            var orders = await _orderAdapter.GetOrdersByStatusesAsync(new[] { OrderStatus.InProgress }, cancellationToken);

            var bestSellerProducts = _marketingService.GetBestSellerProducts(orders)
                .ToDictionary(x => x.ProductId, x => x);

            var bestSellerProductIds = bestSellerProducts.Select(x => x.Key).ToArray();

            var products = (await _productAdapter.GetProductsByIds(bestSellerProductIds, cancellationToken))
                .ToDictionary(x => x.Id, x => x);

            return bestSellerProducts.Select(x => new BestSellerProductDto
            {
                ProductId = x.Key,
                GTIN = x.Value.Gtin,
                ProductName = products[x.Key].Name,
                TotalQuantity = x.Value.Quantity
            }).ToList();
        }
    }
}
