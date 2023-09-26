using MyHtml.Configuration;
using MyHttp;
using MyHttp.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HTTPServer
{
    class Server
    {
        HttpListener listener; // Объект, принимающий TCP-клиентов

        HttpListenerContext context;

        public HttpListenerResponse response;

        Appsetting config = Configurationcs.GetConfigurationcs();
        public Server()
        {
            listener = new HttpListener();

            listener.Prefixes.Add($"{config.Address}:{config.Port}/");

            listener.Start();

            Console.WriteLine("Start выполнен");

            context = listener.GetContext();


            var requestUrl = context.Request.Url;

            response = context.Response;

            if (!FolderHandler.FolderIsExist(config.StaticFilePath))
            {
                FolderHandler.CreateFolder(config.StaticFilePath);
            }

            var responseText = FolderHandler.GetHtml(config.StaticFilePath);


            using (StreamReader stream = new StreamReader(responseText))
            {
                responseText = stream.ReadToEnd();
            }
            byte[] buffer = Encoding.UTF8.GetBytes(responseText);
            // получаем поток ответа и пишем в него ответ

            response.ContentLength64 = buffer.Length;

            using (Stream output = response.OutputStream)
            {
                output.WriteAsync(buffer, 0, buffer.Length);
                output.FlushAsync();

                Console.WriteLine("Запрос обработан");
            }
        }

        ~Server()
        {
            if (listener != null)
            {
                listener.Stop();
            }
        }


    }
}
