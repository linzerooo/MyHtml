using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHtml.Configuration
{
    public class EmailISenderService
    {
        public string mailSender { get; set; }
        public string passwordSender { get; set; }

        public string fromEmail { get; set; }
        public string smptServerHost{ get; set; }
        public ushort smtpServerPost { get; set; }
    }
}
