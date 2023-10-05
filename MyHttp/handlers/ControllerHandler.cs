using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyHtml.handlers
{
    public class ControllerHandler : Handler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            // некоторая обработка запроса

            if (context.Request.Url.AbsolutePath.EndsWith(".html"))
            {
                // завершение выполнения запроса;
            }
            // передача запроса дальше по цепи при наличии в ней обработчиков
            else if (Successor != null)
            {
                Successor.HandleRequest(context);


            }
        }
    }
}
