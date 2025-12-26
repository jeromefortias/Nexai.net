namespace Musuko.Kaonashi
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class NewsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _newsUrl;

        public NewsService(string completionHost, int completionPort)
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
            _newsUrl = $"http://{completionHost}:{completionPort}/news";
        }

        public async Task<List<News>> GetNewsAsync(string mode = "test")
        {
            try
            {
                var parameter = new NewsParameter { mode = mode };
                var jsonContent = JsonConvert.SerializeObject(parameter);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_newsUrl, content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var newsList = JsonConvert.DeserializeObject<List<News>>(responseBody);
                    return newsList ?? new List<News>();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"News service returned status: {response.StatusCode}");
                    return new List<News>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error fetching news: {ex.Message}");
                return new List<News>();
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}

































