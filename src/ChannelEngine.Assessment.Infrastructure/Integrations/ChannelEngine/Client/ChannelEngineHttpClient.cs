using ChannelEngine.Assessment.Infrastructure.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ChannelEngine.Assessment.Infrastructure.Integrations.ChannelEngine
{
    internal class ChannelEngineHttpClient : DefaultHttpClient
    {
        private readonly string _apiKey;

        public ChannelEngineHttpClient(HttpClient httpClient, string apiKey) : base(httpClient)
        {
            _apiKey = apiKey;
        }

        public override async Task<TResponse> GetAsync<TResponse>(string path, object query, CancellationToken cancellationToken = default)
        {
            var queryString = ToQueryString(query);

            var httpResponse = await _httpClient.GetAsync(path + queryString + $"&apiKey={_apiKey}", cancellationToken)
                .ConfigureAwait(false);

            return await DeserializeResponseAsync<TResponse>(httpResponse, cancellationToken);
        }

        public override async Task<TResponse> GetAsync<TResponse>(string path, string query, CancellationToken cancellationToken = default)
        {
            var httpResponse = await _httpClient.GetAsync(path + query + $"&apiKey={_apiKey}", cancellationToken)
               .ConfigureAwait(false);

            return await DeserializeResponseAsync<TResponse>(httpResponse, cancellationToken);
        }

        public override async Task<TResponse> PostAsync<TRequest, TResponse>(string path, TRequest request, CancellationToken cancellationToken)
        {
            var requestBody = JsonConvert.SerializeObject(request);

            var httpResponse = await _httpClient.PostAsync(path + $"?apiKey={_apiKey}", new StringContent(requestBody), cancellationToken)
                .ConfigureAwait(false);

            return await DeserializeResponseAsync<TResponse>(httpResponse, cancellationToken);
        }

        public override Task<bool> DeleteAsync(string path, CancellationToken cancellationToken = default)
        {
            return base.DeleteAsync(path + $"?apiKey={_apiKey}", cancellationToken);
        }

        public override async Task<TResponse> PatchAsync<TRequest, TResponse>(string path, TRequest request, CancellationToken cancellationToken = default)
        {
            var requestBody = JsonConvert.SerializeObject(request);

            /// JSONPatch issues
            /// When path starting with "/", API not updating value
            /// When request contains array API returning error: "The index value provided by path segment '0' is out of bounds of the array size.",
            /*
             [
              {
                "value": "test",
                "path": "/ExtraData/0/Value",
                "op": "replace"
              },
              {
                "value": 11,
                "path": "/Stock",
                "op": "replace"
              }
            ]
            */
            var httpResponse = await _httpClient.PatchAsync(path + $"?apiKey={_apiKey}", new StringContent(requestBody, null, "application/json-patch+json"), cancellationToken)
                .ConfigureAwait(false);

            return await DeserializeResponseAsync<TResponse>(httpResponse, cancellationToken);
        }

        public override async Task<TResponse> PutAsync<TRequest, TResponse>(string path, TRequest request, CancellationToken cancellationToken = default)
        {
            var requestBody = JsonConvert.SerializeObject(request);

            var httpResponse = await _httpClient.PutAsync(path + $"?apiKey={_apiKey}", new StringContent(requestBody), cancellationToken)
                .ConfigureAwait(false);

            return await DeserializeResponseAsync<TResponse>(httpResponse, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
