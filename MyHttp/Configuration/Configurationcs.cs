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
        Appsetting config = new Appsetting();
        public Appsetting? GetConfigurationcs()
        {
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
                Console.WriteLine("работа завершена");
            }
            return config;
        }
    }
}
