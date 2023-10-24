//using HtmlAgilityPack;
//using MimeKit.Text;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Serialization;
//using HtmlAttribute = HtmlAgilityPack.HtmlAttribute;

//namespace MyHtml
//{
//    public class SteamServer
//    {
//        public static WebClient wClient;
//        public static WebRequest request;
//        public static WebResponse response;

//        public static List<SteamCommunity> steamList;

//        public static Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

//        static int GetPagesCount(HtmlDocument html)
//        {
//            var liNodes = html.GetElementbyId("nav-pages").ChildNodes.Where(x => x.Name == "li");

//            HtmlAttribute href = liNodes.Last().FirstChild.Attributes["href"];

//            int pagesCount = (int)Char.GetNumericValue(href.Value[href.Value.Length - 2]);

//            return pagesCount;

//        }

//        static void GetJobLinks(HtmlDocument html)
//        {
//            var trNo = html.GetElementbyId("title");
//            var trNod = trNo.ChildNodes;
//            var trNodes = trNod.Where(x => x.Name == "tr");

//            foreach (var item in trNodes)
//            {
//                var tdNodes = item.ChildNodes.Where(x => x.Name == "td").ToArray();
//                if (tdNodes.Count() != 0)
//                {
//                    var location = tdNodes[2].ChildNodes.Where(x => x.Name == "a").ToArray();

//                    steamList.Add(new SteamCommunity()
//                    {
//                        Url = tdNodes[0].ChildNodes.First().Attributes["href"].Value,
//                        price = tdNodes[0].FirstChild.InnerText,
//                        Discription = tdNodes[1].FirstChild.InnerText,
//                        Name = location[0].InnerText,
//                        Count = location[2].InnerText
//                    });
//                }

//            }
//        }
//        static void GetFullInfo(SteamCommunity steam)
//        {
//            HtmlDocument html = new HtmlDocument();
//            // html.LoadHtml(wClient.DownloadString(job.Url));
//            html.LoadHtml(GetHtmlString(steam.Url));

//            // так делать нельзя :-(
//            var table = html.GetElementbyId("main-content").ChildNodes[1].ChildNodes[9].ChildNodes[1].ChildNodes[2].ChildNodes[1].ChildNodes[3].ChildNodes.Where(x => x.Name == "tr").ToArray();

//            foreach (var tr in table)
//            {
//                string category = tr.ChildNodes.FindFirst("th").InnerText;

//                switch (category)
//                {
//                    case "Цена":
//                        steam.price = tr.ChildNodes.FindFirst("td").FirstChild.InnerText;
//                        break;
//                    case "Название":
//                        steam.Name = tr.ChildNodes.FindFirst("td").InnerText;
//                        break;
//                    case "Описание":
//                        steam.Discription = tr.ChildNodes.FindFirst("td").InnerText;
//                        break;
//                    default:
//                        continue;
//                }
//            }
//        }

//        public static string GetHtmlString(string url)
//        {
//            request = WebRequest.Create(url);
//            request.Proxy = null;
//            response = request.GetResponse();
//            using (StreamReader sReader = new StreamReader(response.GetResponseStream(), encode))
//            {
//                return sReader.ReadToEnd();
//            }
//        }

//        public static void SerializeToXml(List<SteamCommunity> jobList)
//        {
//            using (TextWriter output = new StreamWriter("report.xml"))
//            {
//                XmlSerializer serializer = new XmlSerializer(typeof(List<SteamCommunity>));
//                serializer.Serialize(output, jobList);
//            }
//        }

//        static void Main(string[] args)
//        {
//            steamList = new List<SteamCommunity>();
//            wClient = new WebClient();

//            wClient.Proxy = null;
//            wClient.Encoding = encode;

//            HtmlDocument html = new HtmlDocument();

//            html.LoadHtml(wClient.DownloadString("https://career.habr.com/vacancies/"));
//            GetJobLinks(html);

//            int pagesCount = GetPagesCount(html);

//            for (int i = 2; i <= pagesCount; i++)
//            {
//                html.LoadHtml(wClient.DownloadString(string.Format("http://habrahabr.ru/job/page{0}", 1000121656)));
//                GetJobLinks(html);
//            }

//            foreach (var steam in steamList)
//            {
//                GetFullInfo(steam);
//            }

//            SerializeToXml(steamList);
//        }
//    }
//}