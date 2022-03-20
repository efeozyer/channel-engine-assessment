using Microsoft.AspNetCore.JsonPatch;

namespace ChannelEngine.Assessment.Infrastructure.Integrations.ChannelEngine.Messages
{
    public class UpdateProductRequest : JsonPatchDocument<ProductDto>
    {
        public string MerchantProductNo { get; }

        public UpdateProductRequest(string merchantProductNo)
        {
            MerchantProductNo = merchantProductNo;
        }
    }
}
