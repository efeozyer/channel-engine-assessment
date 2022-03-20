using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ChannelEngine.Assessment.Domain.Marketing.Models;
using ChannelEngine.Assessment.Infrastructure.Integrations.ChannelEngine;
using ChannelEngine.Assessment.Infrastructure.Integrations.ChannelEngine.Messages;

namespace ChannelEngine.Assessment.Acl.ChannelEngine
{
    public class ProductAdapter : IProductAdapter
    {
        private readonly IChannelEngineClient _channelEngineClient;

        public ProductAdapter(IChannelEngineClient channelEngineClient)
        {
            _channelEngineClient = channelEngineClient;
        }

        public async Task<List<Product>> GetProductsByIds(string[] productIds, CancellationToken cancellationToken = default)
        {
            var request = new GetProductsByFilterRequest
            {
                MerchantProductNoList = productIds.ToList()
            };

            var response = await _channelEngineClient.GetProductsByFilterAsync(request, cancellationToken);

            return response.Content.Select(x => Map(x)).ToList();
        }

        #region Private methods
        private Product Map(ProductDto product)
        {
            return new Product
            {
                Id = product.MerchantProductNo,
                Name = product.Name,
                Price = product.Price
            };
        }
        #endregion
    }
}
