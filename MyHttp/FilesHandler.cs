using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml.Schema;
using HTTPServer;
using MyHtml.Configuration;

namespace MyHttp
{
    public static class FilesHandler
    {
        public static bool FolderIsExist(string path)
        {
            return File.Exists(path);
        }
        public static void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }

        public static string? GetHtml(string path) 
        {
            if (File.Exists(path + "google.html"))
            {
                return path + "index.html";
            }
            else
            {
                return path + "index.html";
            }            
        }
    }
}
