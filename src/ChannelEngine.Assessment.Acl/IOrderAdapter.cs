using ChannelEngine.Assessment.Domain.Marketing.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChannelEngine.Assessment.Acl
{
    public interface IOrderAdapter
    {
        public Task<List<Order>> GetOrdersByStatusesAsync(OrderStatus[] orderStatuses, CancellationToken cancellationToken = default);
    }
}
