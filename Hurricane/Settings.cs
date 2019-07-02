using System.Collections.Generic;
using System.Configuration;

namespace Hurricane
{
    public class Settings
    {
        public static readonly string API_TOKEN = ConfigurationSettings.AppSettings["ApiToken"];
        public static readonly string URL_USER_WEEK = ConfigurationSettings.AppSettings["UrlUserWeek"];
        public static readonly string URL_ALL_USERS = ConfigurationSettings.AppSettings["UrlAllUsers"];     
        public static readonly string IS_MOKED = ConfigurationSettings.AppSettings["IsMoked"];

        public static readonly HashSet<long> ALLOWED_CHATS = new HashSet<long>
        {
            248580711,
            438265830,
            110638968,
            98331770
        };

        public static readonly HashSet<string> ALLOWED_USERS = new HashSet<string>
        {
            "psok",
            "tsok",
            "drud",
            "ovor"
        };
    }
}
