using System.Collections.Generic;

namespace ChannelEngine.Assessment.Domain.Marketing.Models
{
    public class Order
    {
        public List<OrderLine> Lines { get; set; }
        public long Id { get; set; }
        public OrderStatus Status { get; set; }
    }
}
