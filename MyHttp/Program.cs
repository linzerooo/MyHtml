﻿using System.IO;
using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyHttp.Configuration;
using MyHttp;

const string filePath = @".\appsettings.json";
HttpListener server = new HttpListener();
Appsetting config = new Appsetting();
// установка адресов прослушки

try
{
    if (!File.Exists(filePath))
    {
        Console.WriteLine("apssettings.jcon не существует");
        throw new Exception();
    }

    using (var stream = File.OpenRead(filePath))
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

server.Prefixes.Add($"{config.Address}:{config.Port}/");

FolderHandler folderHandler = new FolderHandler();

var responseText = folderHandler.GetHtml(config.StaticFilePath);

if (responseText == null)
{
    Console.WriteLine("html файла не существует");
    if (config.StaticFilePath == null)
    {
        folderHandler.CreateFolder("C:\\Users\\79991\\RiderProjects\\MyHttp\\MyHttp\\static");
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
