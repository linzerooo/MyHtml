//using HTTPServer;
//using MyHtml.Новая_папка;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Principal;
//using System.Text;
//using System.Threading.Tasks;

//namespace MyHtml.handlers
//{
//    [ControllerAttribute("Authorize")]
//    [Post("Authorize")]
//    [Send("Authorize")]


//    public class Authorize
//    {
//        public void GetEmailList(string email, string password)
//        {
//            new Server().SendEmailAsync(email, password);
//            Console.WriteLine("письмо отправлено");
//        }
//        public string GetEmailList(string email, string password)
//        {
//            string htmlCode = "<html><body><h1>жопа</h1></body></html>";
//            return htmlCode;
//        }
//        public Account[] GetAccountsList()
//        {
//            var account = new[]
//            {
//                new Account() { Email = "email", Password = "password"}
//            };
//            return new Account[] { };
//        }
//    }
//}
