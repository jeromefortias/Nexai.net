using System;
using System.IO;
using Newtonsoft.Json;

namespace Musuko.Kaonashi
{
    public class AppConfig
    {
        public string OllamaHost { get; set; } = "localhost";
        public int OllamaPort { get; set; } = 11434;
        public string CompletionHost { get; set; } = "localhost";
        public int CompletionPort { get; set; } = 50824;
        public string DefaultModel { get; set; } = "llama2";
        public string LastModelUsed { get; set; } = "";
        public string SystemPrompt { get; set; } = "You are a helpful AI assistant.";
        public bool HideThinkTags { get; set; } = true;
        public bool SaveInteractions { get; set; } = false;
        public string PromptFolder { get; set; } = "";

        private static readonly string ConfigFilePath = 
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                        "Localhost.AI.Kaonashi", "config.json");

        public static AppConfig Load()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    string json = File.ReadAllText(ConfigFilePath);
                    return JsonConvert.DeserializeObject<AppConfig>(json) ?? new AppConfig();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading config: {ex.Message}", "Configuration Error", 
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return new AppConfig();
        }

        public void Save()
        {
            try
            {
                string directory = Path.GetDirectoryName(ConfigFilePath)!;
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(ConfigFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving config: {ex.Message}", "Configuration Error", 
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

