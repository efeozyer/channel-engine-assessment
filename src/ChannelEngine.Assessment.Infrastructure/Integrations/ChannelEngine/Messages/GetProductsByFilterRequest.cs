using System.Collections.Generic;

namespace ChannelEngine.Assessment.Infrastructure.Integrations.ChannelEngine.Messages
{
    public class GetProductsByFilterRequest : PagingRequest
    {
        public string Search { get; set; }

        public List<string> EanList { get; set; }

        public List<string> MerchantProductNoList { get; set; }
    }
}
