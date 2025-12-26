namespace Musuko.Kaonashi
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class OllamaService
    {
        private readonly HttpClient _httpClient;
        private string _baseUrl;
        private string _completionUrl;

        public OllamaService(string host, int port, string completionHost, int completionPort)
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
            UpdateConnectionSettings(host, port, completionHost, completionPort);
        }

        public void UpdateConnectionSettings(string host, int port, string completionHost, int completionPort)
        {
            _baseUrl = $"http://{host}:{port}";
            _completionUrl = $"http://{completionHost}:{completionPort}";
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/tags");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string[]> GetAvailableModelsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/tags");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JObject.Parse(json);
                    var models = data["models"]?.ToObject<JArray>();
                    
                    if (models != null)
                    {
                        var modelNames = new string[models.Count];
                        for (int i = 0; i < models.Count; i++)
                        {
                            modelNames[i] = models[i]["name"]?.ToString() ?? "unknown";
                        }
                        return modelNames;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get models: {ex.Message}");
            }
            return Array.Empty<string>();
        }

        public async Task<string> SendMessageAsync(string message, string model, 
                                                   Action<string> onChunkReceived)
        {
            try
            {
                var requestBody = new
                {
                    model = model,
                    prompt = message,
                    stream = false
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/generate", content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var responseData = JObject.Parse(responseBody);
                var responseText = responseData["response"]?.ToString() ?? string.Empty;

                // Invoke callback with the complete response at once
                if (!string.IsNullOrEmpty(responseText))
                {
                    onChunkReceived?.Invoke(responseText);
                }

                return responseText;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send message: {ex.Message}");
            }
        }

        public string ExtractFirstMessageContent(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return string.Empty;

            try
            {
                using var document = JsonDocument.Parse(json);
                var root = document.RootElement;

                // Navigate to choices[0].message.content
                if (root.TryGetProperty("choices", out var choices) && choices.ValueKind == JsonValueKind.Array)
                {
                    var firstChoice = choices[0];
                    if (firstChoice.TryGetProperty("message", out var message) &&
                        message.TryGetProperty("content", out var content))
                    {
                        return content.GetString() ?? string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Invalid JSON: {ex.Message}");
            }

            return string.Empty;
        }

        public async Task<string> SendChatMessageAsync(string message, string model, 
                                                       ChatHistory history,
                                                       Action<string> onChunkReceived,
                                                       string? systemPrompt = null)
        {
            try
            {
                var messages = history.GetMessages(systemPrompt);
                messages.Add(new { role = "user", content = message });

                var requestBody = new
                {
                    model = model,
                    messages = messages,
                    stream = false
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Use completion URL for chat
                var response = await _httpClient.PostAsync($"{_completionUrl}/v1/chat/completions", content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var responseData = JObject.Parse(responseBody);
                string tt = ExtractFirstMessageContent((string)responseBody);


                var messageData = responseData["message"];
                var assistantResponse = tt;

                // Invoke callback with the complete response at once
                if (!string.IsNullOrEmpty(assistantResponse))
                {
                    onChunkReceived?.Invoke(assistantResponse);
                }

                history.AddMessage("user", message);
                history.AddMessage("assistant", assistantResponse);

                return assistantResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send chat message: {ex.Message}");
            }
        }
    }

    public class ChatHistory
    {
        private readonly List<dynamic> _messages = new List<dynamic>();

        public void AddMessage(string role, string content)
        {
            _messages.Add(new { role, content });
        }

        public List<dynamic> GetMessages(string? systemPrompt = null)
        {
            var messages = new List<dynamic>();
            
            // Add system prompt as first message if provided
            if (!string.IsNullOrWhiteSpace(systemPrompt))
            {
                messages.Add(new { role = "system", content = systemPrompt });
            }
            
            // Add all conversation messages
            messages.AddRange(_messages);
            
            return messages;
        }

        public void Clear()
        {
            _messages.Clear();
        }
    }
}

