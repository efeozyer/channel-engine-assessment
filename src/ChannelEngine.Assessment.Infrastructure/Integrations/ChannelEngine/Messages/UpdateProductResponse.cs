using System.Collections.Generic;

namespace ChannelEngine.Assessment.Infrastructure.Integrations.ChannelEngine.Messages
{
    public class UpdateProductResponse
    {
        public int RejectedCount { get; set; }
        public int AcceptedCount { get; set; }
        public List<ProductMessageDto> ProductMessages { get; set; }
    }
}
