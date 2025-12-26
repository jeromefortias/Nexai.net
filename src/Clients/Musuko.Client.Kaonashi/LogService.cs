namespace Musuko.Kaonashi
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class LogService
    {
        private readonly string _logUrl;
        private static readonly HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(5)
        };

        public LogService(string completionHost, int completionPort)
        {
            _logUrl = $"http://{completionHost}:{completionPort}/log";
        }

        public async Task LogAsync(string type, string comment)
        {
            try
            {
                var log = new Log
                {
                    AppName = "Kaonashi Client",
                    Type = type,
                    Date = DateTime.Now,
                    MachineName = Environment.MachineName,
                    UserName = Environment.UserName,
                    Comment = comment
                };

                var jsonContent = JsonConvert.SerializeObject(log);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync(_logUrl, content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var serviceReturn = JsonConvert.DeserializeObject<ServiceReturn>(responseBody);
                    System.Diagnostics.Debug.WriteLine($"Log recorded: {serviceReturn?.Message}");
                }
            }
            catch (Exception ex)
            {
                // Don't throw - logging should never break the app
                System.Diagnostics.Debug.WriteLine($"Failed to log: {ex.Message}");
            }
        }

        public void Log(string type, string comment)
        {
            // Fire and forget for synchronous contexts
            _ = LogAsync(type, comment);
        }
    }
}

