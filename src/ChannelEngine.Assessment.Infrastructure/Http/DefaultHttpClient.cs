using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ChannelEngine.Assessment.Infrastructure.Http
{
    /// <summary>
    /// Default Http Client implementation
    /// Provides centeralized logging for HttpClient
    /// </summary>
    public class DefaultHttpClient : IDisposable
    {
        private readonly ILogger _logger = Log.ForContext<DefaultHttpClient>();

        private readonly HttpClient _httpClient;
        private bool _disposedValue;

        public DefaultHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<TResponse> GetAsync<TResponse>(string path, string query, CancellationToken cancellationToken = default)
        {
            var httpResponse = await _httpClient.GetAsync(path + query, cancellationToken)
                .ConfigureAwait(false);

            return await DeserializeResponseAsync<TResponse>(httpResponse, cancellationToken);
        }

        public async Task<TResponse> GetAsync<TResponse>(string path, object query, CancellationToken cancellationToken = default)
        {
            var queryString = ToQueryString(query);

            var httpResponse = await _httpClient.GetAsync(path + queryString, cancellationToken)
                .ConfigureAwait(false);

            return await DeserializeResponseAsync<TResponse>(httpResponse, cancellationToken);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string path, TRequest request, CancellationToken cancellationToken)
            where TRequest : class
        {
            var requestBody = JsonConvert.SerializeObject(request);

            var httpResponse = await _httpClient.PostAsync(path, new StringContent(requestBody), cancellationToken)
                .ConfigureAwait(false);

            return await DeserializeResponseAsync<TResponse>(httpResponse, cancellationToken);
        }

        public async Task<bool> DeleteAsync(string path, CancellationToken cancellationToken = default)
        {
            var httpResponse = await _httpClient.DeleteAsync(path, cancellationToken)
                .ConfigureAwait(false);

            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<TResponse> PatchAsync<TRequest, TResponse>(string path, TRequest request, CancellationToken cancellationToken = default)
            where TRequest : class, IJsonPatchDocument
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
            var httpResponse = await _httpClient.PatchAsync(path, new StringContent(requestBody, null, "application/json-patch+json"), cancellationToken)
                .ConfigureAwait(false);

            return await DeserializeResponseAsync<TResponse>(httpResponse, cancellationToken);
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(string path, TRequest request, CancellationToken cancellationToken = default)
            where TRequest : class
        {
            var requestBody = JsonConvert.SerializeObject(request);

            var httpResponse = await _httpClient.PutAsync(path, new StringContent(requestBody), cancellationToken)
                .ConfigureAwait(false);

            return await DeserializeResponseAsync<TResponse>(httpResponse, cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task<TResponse> DeserializeResponseAsync<TResponse>(HttpResponseMessage httpResponse, CancellationToken cancellationToken)
        {
            if (!httpResponse.IsSuccessStatusCode)
            {
                _logger.Error($"Request failed with code {httpResponse.StatusCode}");
            }

            var jsonBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken)
                .ConfigureAwait(false);

            try
            {
                var response = JsonConvert.DeserializeObject<TResponse>(jsonBody);

                return response;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Serialization failed for {typeof(TResponse).FullName}");
            }

            return default(TResponse);
        }

        public static string ToQueryString(object request, string separator = ",")
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var properties = request.GetType().GetProperties()
                .Where(x => x.CanRead)
                .Where(x => x.GetValue(request, null) != null)
                .ToDictionary(x => x.Name, x => x.GetValue(request, null));

            var propertyNames = properties
                .Where(x => !(x.Value is string) && x.Value is IEnumerable)
                .Select(x => x.Key)
                .ToList();

            foreach (var key in propertyNames)
            {
                var valueType = properties[key].GetType();
                var valueElemType = valueType.IsGenericType
                                        ? valueType.GetGenericArguments()[0]
                                        : valueType.GetElementType();
                if (valueElemType.IsPrimitive || valueElemType == typeof(string))
                {
                    var enumerable = properties[key] as IEnumerable;
                    properties[key] = string.Join(separator, enumerable.Cast<object>());
                }
            }

            return string.Join("&", properties
                .Select(x => string.Concat(
                    Uri.EscapeDataString(x.Key), "=",
                    Uri.EscapeDataString(x.Value.ToString()))));
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
    }
}
