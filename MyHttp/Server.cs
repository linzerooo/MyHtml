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
        TcpListener Listener; // Объект, принимающий TCP-клиентов
        public Server(Appsetting config)
        {
            Listener = new TcpListener(IPAddress.Any, (int)config.Port);
            Listener.Start();

            // В бесконечном цикле
            while (true)
            {
                // Принимаем новых клиентов. После того, как клиент был принят,
                // он передается в новый поток (ClientThread)
                // с использованием пула потоков.
                ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), Listener.AcceptTcpClient());
            }
        }
        static void ClientThread(Object StateInfo)
        {
            new Client((TcpClient)StateInfo);
        }
        ~Server()
        {
            if (Listener != null)
            {
                Listener.Stop();
            }
        }
    }
}
