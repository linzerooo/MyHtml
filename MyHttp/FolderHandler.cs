using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHttp
{
    public class FolderHandler
    {
        public bool FolderIsExist(string path)
        {
            bool folderExist = File.Exists(path);
            if (folderExist)
            {
                return true;
            }
            return false;
        }
        public void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }
        public string? GetHtml(string path) 
        { 
            return Directory.GetFiles(path, "google.html", SearchOption.AllDirectories).FirstOrDefault();
        }
    }
}
