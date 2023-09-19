using System.Reflection.Metadata;

namespace MyHttp.Configuration;

public class Appsetting
{
    public uint Port { get; set; }
    public string Address { get; set; }
    public string StaticFilePath { get; set; } //хранится html
}