using MyHtml.Configuration;
using MyHttp;
using MyHttp.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;

namespace HTTPServer //idisposable
{
    class Server
    {
        HttpListener listener; // Объект, принимающий TCP-клиентов

        HttpListenerContext context;
        private HttpListenerRequest request;
        public HttpListenerResponse response;

        Appsetting config = Configurationcs.GetConfigurationcs();

        public Server()
        {
            listener = new HttpListener();

            listener.Prefixes.Add($"{config.Address}:{config.Port}/");

            listener.Start();

            Console.WriteLine("Start выполнен");

            context = listener.GetContext();
            request = context.Request;
            var abslPath = request.Url.AbsolutePath;


            try
            {
                if (request.HttpMethod.Equals("post", StringComparison.OrdinalIgnoreCase) && abslPath == "/send-email")
                {
                    var stream = new StreamReader(request.InputStream);
                    //var str = stream.ReadToEndAsync();
                    string[] str = stream.ReadToEndAsync().ToString().Split("&") ;
                    SendEmailAsync(str[0], str[1]);
                }

                if (request.HttpMethod.Equals("post", StringComparison.OrdinalIgnoreCase) && abslPath == "/send-email")
                {
                    var stream = new StreamReader(request.InputStream);
                    //var str = stream.ReadToEndAsync();
                    string[] str = stream.ReadToEndAsync().ToString().Split("&");
                    SendEmailAsync(str[0], str[1]);
                }

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
            catch{

            }



        }
        ~Server()
        {
            //if (listener != null)
            //{
            //    listener.Stop();
            //}

            while (Console.ReadLine() != "stop")
            {
                listener?.Stop();
            }
        }

        //public void Dispose()
        //{

        //}
        private static async Task SendEmailAsync(string email, string password)
        {
            EmailISenderService emailConfig = Configurationcs.GetEmailConfig();

            string mailSender = emailConfig.mailSender;
            string passwordSender = emailConfig.passwordSender;

            var fromEmail = emailConfig.fromEmail;
            string fromName = "Tom";
            string toEmail = "somemail@gmail.com";
            string subject = "subject";
            string body = $"<h1>body!!!</h1><p>email{email} password{password}</p>";
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
    }
}
