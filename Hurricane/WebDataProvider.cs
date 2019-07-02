using System;
using System.IO;
using System.Net;

namespace Hurricane
{
    public class WebDataProvider: IDataProvider
    {
        private WebClient webClient;
        private string url;

        public WebDataProvider(string login, string password)
        {
            this.url = Settings.URL_USER_WEEK;
            webClient = new WebClient();
            webClient.Credentials = new NetworkCredential(login, password);
        }

        public string GetUserData(string login)
        {
            string userDataHtmlPage = GetUserDataHtmlPage(login);
            return HtmlParser.ParseUserData(userDataHtmlPage);
        }

        private string GetUserDataHtmlPage(string login)
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
    }
}
