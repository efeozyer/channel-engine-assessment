using ChannelEngine.Assessment.Domain.Marketing.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChannelEngine.Assessment.Acl
{
    public interface IProductAdapter
    {
        Task<List<Product>> GetProductsByIds(string[] productIds, CancellationToken cancellationToken = default);
    }
}
