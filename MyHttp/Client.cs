// Класс-обработчик клиента
using HTTPServer;
using MyHtml.Configuration;
using MyHttp;
using MyHttp.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
    public static Appsetting? config = Configurationcs.GetConfigurationcs();

    //FolderHandler folderHandler = new FolderHandler();

    public string? responseText = FolderHandler.GetHtml(config.StaticFilePath);

    // Конструктор класса. Ему нужно передавать принятого клиента от TcpListener
    public Client()
    {
        //// Код простой HTML-странички

        //// Приведем строку к виду массива байт

        //using (StreamReader stream = new StreamReader(responseText))
        //{
        //    responseText = stream.ReadToEnd();
        //}

        //byte[] buffer = Encoding.UTF8.GetBytes(responseText);
    }

}   