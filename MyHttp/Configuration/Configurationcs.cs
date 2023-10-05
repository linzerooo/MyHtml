using MyHttp.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyHtml.Configuration
{
    public class Configurationcs
    {
        const string jsonPath = @".\appsettings.json";
        public static Appsetting? GetConfigurationcs()
        {
            Appsetting config = new Appsetting();
            try
            {
                if (!File.Exists(jsonPath))
                {
                    Console.WriteLine("apssettings.jcon не существует");
                    throw new Exception();
                }

                using (var stream = File.OpenRead(jsonPath))
                {
                    config = JsonSerializer.Deserialize<Appsetting>(stream);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("работа ");
            }
            return config;
        }
        public static EmailISenderService? GetEmailConfig()
        {
            EmailISenderService config = new EmailISenderService();
            try
            {
                if (!File.Exists(jsonPath))
                {
                    Console.WriteLine("apssettings.jcon не существует");
                    throw new Exception();
                }

                using (var stream = File.OpenRead(jsonPath))
                {
                    config = JsonSerializer.Deserialize<EmailISenderService>(stream);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("работа ");
            }
            return config;
        }
    }
}
