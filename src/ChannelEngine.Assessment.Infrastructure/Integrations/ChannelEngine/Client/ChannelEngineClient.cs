using Serilog;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using ChannelEngine.Assessment.Infrastructure.Http;
using ChannelEngine.Assessment.Infrastructure.Integrations.ChannelEngine.Messages;

namespace ChannelEngine.Assessment.Infrastructure.Integrations.ChannelEngine
{
    public class ChannelEngineClient : IChannelEngineClient, IDisposable
    {
        private readonly DefaultHttpClient _httpClient;

        private readonly ILogger _logger = Log.ForContext<ChannelEngineClient>();
        private readonly ChannelEngineClientConfig _clientConfig;
        private bool _disposedValue;

        public ChannelEngineClient(HttpClient httpClient, ChannelEngineClientConfig clientConfig)
        {
            _clientConfig = clientConfig ?? throw new ArgumentNullException(nameof(clientConfig));

            httpClient.BaseAddress = new Uri(clientConfig.ServiceUrl);

            _httpClient = new ChannelEngineHttpClient(httpClient, _clientConfig.ApiKey);
        }

        public async Task<PagingResponse<OrderDto>> GetOrdersByFilterAsync(GetOrdersByFilterRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync<PagingResponse<OrderDto>>($"orders", request, cancellationToken);

            return ValidateClientResponse(request, response);
        }

        public async Task<PagingResponse<ProductDto>> GetProductsByFilterAsync(GetProductsByFilterRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync<PagingResponse<ProductDto>>($"products", request, cancellationToken);

            return ValidateClientResponse(request, response);
        }

        public async Task<ApiResponse<UpdateProductResponse>> UpdateProductByIdAsync(UpdateProductRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PatchAsync<UpdateProductRequest, ApiResponse<UpdateProductResponse>>($"products/{request.MerchantProductNo}", request, cancellationToken);

            return ValidateClientResponse(request, response);
        }

        #region Private methods

        private TResponse ValidateClientResponse<TResponse>(object request, TResponse response)
            where TResponse : ApiResponse
        {
            if (response == null || !response.Success || response.ValidationErrors.Any() || !string.IsNullOrWhiteSpace(response.Message))
            {
                _logger.Error($"{_clientConfig.Name} : An error occourd ({request.GetType().FullName}) details : {response.Message}");

                throw new HttpRequestException("Request failed!");
            }

            return response;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _httpClient.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
