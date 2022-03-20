using ChannelEngine.Assessment.Domain.Marketing.Models;
using System.Collections.Generic;
using System.Linq;

namespace ChannelEngine.Assessment.Domain.Marketing.Services
{
    public interface IMarketingService
    {
        List<string> GetBestSellerProductIds(List<Order> orders, int count = 5);
    }

    public class MarketingService : IMarketingService
    {
        public List<string> GetBestSellerProductIds(List<Order> orders, int count = 5)
        {
            return orders.SelectMany(x => x.Lines)
                 .OrderByDescending(x => x.Quantity)
                 .Select(x => x.ProductId)
                 .Distinct()
                 .Take(count)
                 .ToList();
        }
    }
}
