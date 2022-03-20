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
            var orders = await _orderAdapter.GetOrdersByStatusesAsync(new[] { OrderStatus.InProgress });

            var orderLines = orders.SelectMany(x => x.Lines).ToDictionary(x => x.ProductId, x => x);

            var bestSellerProductIds = _marketingService.GetBestSellerProductIds(orders);

            var products = await _productAdapter.GetProductsByIds(bestSellerProductIds.ToArray(), cancellationToken);

            return products.Select(x => new BestSellerProductDto
            {
                GTIN = orderLines[x.Id].Gtin,
                ProductName = x.Name,
                TotalQuantity = orderLines.Where(f => f.Value.ProductId == x.Id).Sum(x => x.Value.Quantity)
            }).ToList();
        }
    }
}
