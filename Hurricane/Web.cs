using System;
using System.IO;
using System.Net;

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

        public string GetUserDataHtmlPage(string login)
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
