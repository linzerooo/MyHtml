// Класс-обработчик клиента
using MyHtml.Configuration;
using MyHttp;
using MyHttp.Configuration;
using System.Net.Sockets;
using System.Text;

class Client
{
    public Appsetting? config = new Configurationcs().GetConfigurationcs();
    // Конструктор класса. Ему нужно передавать принятого клиента от TcpListener
    public Client(TcpClient Client)
    {
        FolderHandler folderHandler = new FolderHandler();
        // Код простой HTML-странички
        string? Html = folderHandler.GetHtml(config.StaticFilePath);

        // Приведем строку к виду массива байт

        byte[] Buffer = Encoding.UTF8.GetBytes(Html);

        // Отправим его клиенту
        Client.GetStream().Write(Buffer, 0, Buffer.Length);
        // Закроем соединение
        Client.Close();
    }
    
}   