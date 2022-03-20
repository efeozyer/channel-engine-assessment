using ChannelEngine.Assessment.Infrastructure.Integrations.ChannelEngine.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace ChannelEngine.Assessment.Infrastructure.Integrations.ChannelEngine
{
    public interface IChannelEngineClient
    {
        /// <summary>
        /// Fetch orders based on the provided OrderFilter.
        /// </summary>
        Task<PagingResponse<OrderDto>> GetOrdersByFilterAsync(GetOrdersByFilterRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve all products.
        /// </summary>
        Task<PagingResponse<ProductDto>> GetProductsByFilterAsync(GetProductsByFilterRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Patch products. This endpoint allows you to update single fields on a product using patch operations, without having to supply the other product information.
        /// </summary>
        Task<ApiResponse<UpdateProductResponse>> UpdateProductByIdAsync(UpdateProductRequest request, CancellationToken cancellationToken = default);
    }
}
