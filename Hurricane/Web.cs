using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Hurricane
{
    public class Web
    {
        private WebClient webClient;
        private string url;

        public Web(string url, string login, string password)
        {
            this.url = url;
            webClient = new WebClient();
            webClient.Credentials = new NetworkCredential(login, password);
        }

        public string GetUserData(string login)
        {
            string htmlPage = GetHtmlPage(login);

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlPage);

            Dictionary<string, string> userData = GetUserDataFromDocument(htmlDoc);         

            return FormatUserData(userData);
        }

        private string GetHtmlPage(string login)
        {
            string urlWithLogin = url + login;
            try
            {
                using (Stream stream = webClient.OpenRead(new Uri(urlWithLogin)))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (WebException e)
            {
                throw e;
            }
        }

        private HtmlNode GetHtmlSubNodeById(HtmlNode htmlNode,
                                        string tagName,
                                        string id)
        {
            return htmlNode.Descendants(tagName)
                    .Where(node => node.GetAttributeValue("id", "")
                        .Equals(id))
                    .First();
        }

        private Dictionary<string, string> GetUserDataFromDocument(HtmlDocument htmlDoc)
        {
            Dictionary<string, string> userData = new Dictionary<string, string>();

            HtmlNode tableNode = GetHtmlSubNodeById(htmlDoc.DocumentNode,
                                    "div",
                                    "ctl00_PlaceHolderMain_UpdPnl_Week");

            userData.Add("Status",
                         GetHtmlSubNodeById(tableNode,
                            "img",
                            "ctl00_PlaceHolderMain_GridView_WeeklyTimeClock_ctl02_StatusImage")
                        .GetAttributeValue("alt", "")
            );

            userData.Add("Diff",
                        GetHtmlSubNodeById(tableNode,
                            "span",
                            "ctl00_PlaceHolderMain_GridView_WeeklyTimeClock_ctl02_Label11")
                        .InnerText
            );

            userData.Add("Actual",
                        GetHtmlSubNodeById(tableNode,
                            "span",
                            "ctl00_PlaceHolderMain_GridView_WeeklyTimeClock_ctl02_Label8")
                        .InnerText
            );

            userData.Add("Expected",
                        GetHtmlSubNodeById(tableNode,
                            "span",
                            "ctl00_PlaceHolderMain_GridView_WeeklyTimeClock_ctl02_Label9")
                        .InnerText
            );

            userData.Add("Mon",
                        GetHtmlSubNodeById(tableNode,
                            "span",
                            "ctl00_PlaceHolderMain_GridView_WeeklyTimeClock_ctl02_Label1")
                        .InnerText
            );

            userData.Add("Tue",
                        GetHtmlSubNodeById(tableNode,
                            "span",
                            "ctl00_PlaceHolderMain_GridView_WeeklyTimeClock_ctl02_Label2")
                        .InnerText
            );

            userData.Add("Wed",
                        GetHtmlSubNodeById(tableNode,
                            "span",
                            "ctl00_PlaceHolderMain_GridView_WeeklyTimeClock_ctl02_Label3")
                        .InnerText
            );

            userData.Add("Thu",
                        GetHtmlSubNodeById(tableNode,
                            "span",
                            "ctl00_PlaceHolderMain_GridView_WeeklyTimeClock_ctl02_Label4")
                        .InnerText
            );

            userData.Add("Fri",
                        GetHtmlSubNodeById(tableNode,
                            "span",
                            "ctl00_PlaceHolderMain_GridView_WeeklyTimeClock_ctl02_Label5")
                        .InnerText
            );

            userData.Add("Sat",
                        GetHtmlSubNodeById(tableNode,
                            "span",
                            "ctl00_PlaceHolderMain_GridView_WeeklyTimeClock_ctl02_Label6")
                        .InnerText
            );

            return userData;
        }

        private string FormatUserData(Dictionary<string, string> userData)
        {
            StringBuilder userDataFormatted = new StringBuilder();

            foreach (var item in userData)
            {
                userDataFormatted.Append(string.Format("{0, -10} {1, -1}\r\n", item.Key, item.Value));
            }

            return userDataFormatted.ToString();
        }
    }
}
