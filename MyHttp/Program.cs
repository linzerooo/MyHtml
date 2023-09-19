using System.IO;
using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyHttp.Configuration;
using MyHttp;
using System.Net.Sockets;
using MyHtml.Configuration;



const string staticFolderPath = @"C:\Users\79991\RiderProjects\MyHttp\MyHttp\static";
HttpListener server = new HttpListener();

Appsetting? config = new Configurationcs().GetConfigurationcs();


// Определим нужное максимальное количество потоков
// Пусть будет по 4 на каждый процессор
int MaxThreadsCount = Environment.ProcessorCount * 4;
// Установим максимальное количество рабочих потоков
ThreadPool.SetMaxThreads(MaxThreadsCount, MaxThreadsCount);
// Установим минимальное количество рабочих потоков
ThreadPool.SetMinThreads(2, 2); 


server.Prefixes.Add($"{config.Address}:{config.Port}/");

FolderHandler folderHandler = new FolderHandler();

var responseText = folderHandler.GetHtml(config.StaticFilePath);

if (responseText == null)
{
    Console.WriteLine("html файла не существует");
    if (config.StaticFilePath == null)
    {
        folderHandler.CreateFolder(staticFolderPath);
    }
}

server.Start(); // начинаем прослушивать входящие подключения

await Console.Out.WriteLineAsync("Start пошёл");
// получаем контекст
var context = await server.GetContextAsync();

var response = context.Response;
// отправляемый в ответ код htmlвозвращает

using (StreamReader stream = new StreamReader(responseText))
{
    responseText = stream.ReadToEnd();
}

byte[] buffer = Encoding.UTF8.GetBytes(responseText);
// получаем поток ответа и пишем в него ответ
response.ContentLength64 = buffer.Length;

using (Stream output = response.OutputStream)
{
    await output.WriteAsync(buffer, 0, buffer.Length);
    await output.FlushAsync();

    Console.WriteLine("Запрос обработан");
}
server.Stop();

static void Main(string[] args)
{

}
