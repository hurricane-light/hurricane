using System.Configuration;

namespace Hurricane
{
    public class Settings
    {
        public static readonly string API_TOKEN = ConfigurationSettings.AppSettings["ApiToken"];
        public static readonly string URL_USER_WEEK = ConfigurationSettings.AppSettings["UrlUserWeek"];
        public static readonly string IS_MOKED = ConfigurationSettings.AppSettings["IsMoked"];
    }
}
