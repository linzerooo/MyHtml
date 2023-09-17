namespace MyHttp.Configuration
{
    public class StaticFilePath
    {
        public StaticFilePath(string path)
        {
            string[] allFoundFiles = Directory.GetFiles(path, "google.html", SearchOption.AllDirectories);

        }

    }
}