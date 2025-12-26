using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Musuko.Kaonashi
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Log application startup
            LogApplicationStartup().GetAwaiter().GetResult();
            
            // Run application in systray mode
            Application.Run(new SystrayApplicationContext());
        }

        private static async Task LogApplicationStartup()
        {
            try
            {
                var config = AppConfig.Load();
                var logService = new LogService(config.CompletionHost, config.CompletionPort);
                await logService.LogAsync("INFO", "Application started in systray mode");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to log startup: {ex.Message}");
            }
        }
    }
}


