namespace Nexai.net.Test.Console
{

    using System;
    using Nexai.net.Core;
    using Nexai.net.Core.Config.Models;
    using Nexai.net.Core.Config.Helpers;

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            
            ConfigCommon config = new ConfigCommon();
            try
            {
                config = ConfigHelper.Get("config.json");
                Console.WriteLine("Config loaded");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Session session = new Session(config);
            Console.WriteLine(session.startAt.ToString());
            Console.ReadKey();
        }
    }
}