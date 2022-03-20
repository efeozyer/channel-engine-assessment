using ChannelEngine.Assessment.Domain.Marketing.Models;
using System.Collections.Generic;
using System.Linq;

namespace ChannelEngine.Assessment.Domain.Marketing.Services
{
    public interface IMarketingService
    {
        List<OrderLine> GetBestSellerProductIds(List<Order> orders, int count = 5);
    }

    public class MarketingService : IMarketingService
    {
        public List<OrderLine> GetBestSellerProductIds(List<Order> orders, int count = 5)
        {
            return orders.SelectMany(x => x.Lines)
                  .GroupBy(x => x.ProductId)
                  .Select(x => new OrderLine
                  {
                      Gtin = x.FirstOrDefault().Gtin,
                      ProductId = x.FirstOrDefault().ProductId,
                      Quantity = x.Sum(s => s.Quantity)
                  })
                  .OrderByDescending(x => x.Quantity)
                  .Take(count)
                  .ToList();
        }
    }
}
