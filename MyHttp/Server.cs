using MyHtml.Configuration;
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

            response = context.Response;

            var responseText = new Client().responseText;

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


            // В бесконечном цикле
            //while (true)
            //{
            //    // Принимаем новых клиентов. После того, как клиент был принят,
            //    // он передается в новый поток (ClientThread)
            //    // с использованием пула потоков.
            //    ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), listener.GetContext()); //listener.AcceptTcpClient());
            //}
        }
        //static void ClientThread(Object StateInfo)
        //{
        //    new Client((TcpClient)StateInfo);
        //}
        ~Server()
        {
            if (listener != null)
            {
                listener.Stop();
            }
        }
        //public async void SendError404()
        //{
        //    string error = @"C:\Users\79991\RiderProjects\MyHttp\MyHttp\static\404.html";
        //    using (StreamReader stream = new StreamReader(error))
        //    {
        //        error = stream.ReadToEnd();
        //    }
        //    byte[] buffer = Encoding.UTF8.GetBytes(error);

        //    using (Stream output = response.OutputStream)
        //    {
        //        await output.WriteAsync(buffer, 0, buffer.Length);
        //        await output.FlushAsync();            
        //    }
        //}

    }
}
