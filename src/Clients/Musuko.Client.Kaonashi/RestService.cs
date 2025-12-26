namespace Musuko.Kaonashi
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    /// <summary>
    /// Provides a centralized abstraction layer for all REST API calls to the application.
    /// </summary>
    public class RestService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the RestService class.
        /// </summary>
        /// <param name="baseHost">The base host for the API (e.g., "localhost")</param>
        /// <param name="basePort">The base port for the API</param>
        /// <param name="timeoutSeconds">Request timeout in seconds (default: 30)</param>
        public RestService(string baseHost, int basePort, int timeoutSeconds = 30)
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(timeoutSeconds)
            };
            _baseUrl = $"http://{baseHost}:{basePort}";
        }

        /// <summary>
        /// Initializes a new instance of the RestService class using AppConfig settings.
        /// </summary>
        /// <param name="timeoutSeconds">Request timeout in seconds (default: 30)</param>
        public RestService(int timeoutSeconds = 30) : this(AppConfig.Load().CompletionHost, AppConfig.Load().CompletionPort, timeoutSeconds)
        {
        }

        /// <summary>
        /// Gets the base URL for the REST service.
        /// </summary>
        public string BaseUrl => _baseUrl;

        /// <summary>
        /// Performs a GET request and returns the deserialized response.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response to</typeparam>
        /// <param name="endpoint">The API endpoint (e.g., "/api/users")</param>
        /// <returns>The deserialized response object</returns>
        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await GetAsync(endpoint);
            return JsonConvert.DeserializeObject<T>(response) ?? throw new InvalidOperationException("Failed to deserialize response");
        }

        /// <summary>
        /// Performs a GET request and returns the raw response string.
        /// </summary>
        /// <param name="endpoint">The API endpoint (e.g., "/api/users")</param>
        /// <returns>The response body as a string</returns>
        public async Task<string> GetAsync(string endpoint)
        {
            var url = BuildUrl(endpoint);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Performs a POST request with a request body and returns the deserialized response.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request object</typeparam>
        /// <typeparam name="TResponse">The type to deserialize the response to</typeparam>
        /// <param name="endpoint">The API endpoint (e.g., "/api/users")</param>
        /// <param name="request">The request object to serialize and send</param>
        /// <returns>The deserialized response object</returns>
        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest request)
        {
            var response = await PostAsync(endpoint, request);
            return JsonConvert.DeserializeObject<TResponse>(response) ?? throw new InvalidOperationException("Failed to deserialize response");
        }

        /// <summary>
        /// Performs a POST request with a request body and returns the raw response string.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request object</typeparam>
        /// <param name="endpoint">The API endpoint (e.g., "/api/users")</param>
        /// <param name="request">The request object to serialize and send (can be null)</param>
        /// <returns>The response body as a string</returns>
        public async Task<string> PostAsync<TRequest>(string endpoint, TRequest request)
        {
            var url = BuildUrl(endpoint);
            HttpResponseMessage response;
            
            if (request != null)
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await _httpClient.PostAsync(url, content);
            }
            else
            {
                response = await _httpClient.PostAsync(url, null);
            }
            
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Performs a POST request without a request body and returns the deserialized response.
        /// </summary>
        /// <typeparam name="TResponse">The type to deserialize the response to</typeparam>
        /// <param name="endpoint">The API endpoint (e.g., "/api/users")</param>
        /// <returns>The deserialized response object</returns>
        public async Task<TResponse> PostAsync<TResponse>(string endpoint)
        {
            var response = await PostAsync(endpoint, (object?)null);
            return JsonConvert.DeserializeObject<TResponse>(response) ?? throw new InvalidOperationException("Failed to deserialize response");
        }

        /// <summary>
        /// Performs a POST request without a request body and returns the raw response string.
        /// </summary>
        /// <param name="endpoint">The API endpoint (e.g., "/api/users")</param>
        /// <returns>The response body as a string</returns>
        public async Task<string> PostAsync(string endpoint)
        {
            return await PostAsync(endpoint, (object?)null);
        }

        /// <summary>
        /// Performs a PUT request with a request body and returns the deserialized response.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request object</typeparam>
        /// <typeparam name="TResponse">The type to deserialize the response to</typeparam>
        /// <param name="endpoint">The API endpoint (e.g., "/api/users/123")</param>
        /// <param name="request">The request object to serialize and send</param>
        /// <returns>The deserialized response object</returns>
        public async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest request)
        {
            var response = await PutAsync(endpoint, request);
            return JsonConvert.DeserializeObject<TResponse>(response) ?? throw new InvalidOperationException("Failed to deserialize response");
        }

        /// <summary>
        /// Performs a PUT request with a request body and returns the raw response string.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request object</typeparam>
        /// <param name="endpoint">The API endpoint (e.g., "/api/users/123")</param>
        /// <param name="request">The request object to serialize and send</param>
        /// <returns>The response body as a string</returns>
        public async Task<string> PutAsync<TRequest>(string endpoint, TRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var url = BuildUrl(endpoint);
            var response = await _httpClient.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Performs a DELETE request and returns the deserialized response.
        /// </summary>
        /// <typeparam name="TResponse">The type to deserialize the response to</typeparam>
        /// <param name="endpoint">The API endpoint (e.g., "/api/users/123")</param>
        /// <returns>The deserialized response object</returns>
        public async Task<TResponse> DeleteAsync<TResponse>(string endpoint)
        {
            var response = await DeleteAsync(endpoint);
            return JsonConvert.DeserializeObject<TResponse>(response) ?? throw new InvalidOperationException("Failed to deserialize response");
        }

        /// <summary>
        /// Performs a DELETE request and returns the raw response string.
        /// </summary>
        /// <param name="endpoint">The API endpoint (e.g., "/api/users/123")</param>
        /// <returns>The response body as a string</returns>
        public async Task<string> DeleteAsync(string endpoint)
        {
            var url = BuildUrl(endpoint);
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Performs a PATCH request with a request body and returns the deserialized response.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request object</typeparam>
        /// <typeparam name="TResponse">The type to deserialize the response to</typeparam>
        /// <param name="endpoint">The API endpoint (e.g., "/api/users/123")</param>
        /// <param name="request">The request object to serialize and send</param>
        /// <returns>The deserialized response object</returns>
        public async Task<TResponse> PatchAsync<TRequest, TResponse>(string endpoint, TRequest request)
        {
            var response = await PatchAsync(endpoint, request);
            return JsonConvert.DeserializeObject<TResponse>(response) ?? throw new InvalidOperationException("Failed to deserialize response");
        }

        /// <summary>
        /// Performs a PATCH request with a request body and returns the raw response string.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request object</typeparam>
        /// <param name="endpoint">The API endpoint (e.g., "/api/users/123")</param>
        /// <param name="request">The request object to serialize and send</param>
        /// <returns>The response body as a string</returns>
        public async Task<string> PatchAsync<TRequest>(string endpoint, TRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var url = BuildUrl(endpoint);
            var requestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = content
            };
            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Builds the full URL from the base URL and endpoint.
        /// </summary>
        /// <param name="endpoint">The API endpoint</param>
        /// <returns>The full URL</returns>
        private string BuildUrl(string endpoint)
        {
            endpoint = endpoint.TrimStart('/');
            return $"{_baseUrl}/{endpoint}";
        }

        /// <summary>
        /// Sets a custom header for all subsequent requests.
        /// </summary>
        /// <param name="name">The header name</param>
        /// <param name="value">The header value</param>
        public void SetHeader(string name, string value)
        {
            _httpClient.DefaultRequestHeaders.Remove(name);
            _httpClient.DefaultRequestHeaders.Add(name, value);
        }

        /// <summary>
        /// Removes a custom header from all subsequent requests.
        /// </summary>
        /// <param name="name">The header name</param>
        public void RemoveHeader(string name)
        {
            _httpClient.DefaultRequestHeaders.Remove(name);
        }

        /// <summary>
        /// Updates the timeout for all subsequent requests.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout in seconds</param>
        public void UpdateTimeout(int timeoutSeconds)
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the RestService and optionally releases the managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the RestService and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _httpClient?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}

