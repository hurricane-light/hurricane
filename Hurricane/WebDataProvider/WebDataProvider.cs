using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Hurricane
{
    public class WebDataProvider: IDataProvider
    {
        private WebClient webClient;
        private string urlAllUsers;
        private string urlUserWeek;

        public WebDataProvider(string login, string password)
        {
            urlAllUsers = Settings.URL_ALL_USERS;
            urlUserWeek = Settings.URL_USER_WEEK;
            webClient = new WebClient();
            webClient.Credentials = new NetworkCredential(login, password);
        }

        public HashSet<string> GetAllUsers()
        {
            string allUsersHtmlPage = GetHtmlPage(urlAllUsers);
            return HtmlParser.ParseAllUsers(allUsersHtmlPage);
        }

        public string GetUserData(string login)
        {
            string userDataHtmlPage = GetHtmlPage(urlUserWeek + login);
            return HtmlParser.ParseUserData(userDataHtmlPage);
        }

        private string GetHtmlPage(string url)
        {
            using (Stream stream = webClient.OpenRead(new Uri(url)))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
