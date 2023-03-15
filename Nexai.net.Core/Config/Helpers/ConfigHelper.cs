
namespace Nexai.net.Core.Config.Helpers
{
    using System;
    using System.Text.Json;
    using Nexai.net.Core.Config.Models;
    using System.IO;
    public class ConfigHelper
    {
        public static ConfigCommon Get(string fullName)
        { 
            ConfigCommon config = new ConfigCommon();

            try
            {
                string json = File.ReadAllText(fullName);
                config = JsonSerializer.Deserialize<ConfigCommon>(json);
                return config;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
