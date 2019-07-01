using System;

namespace Hurricane
{
    public class UserEventArgs: EventArgs
    {
        public string Login { get; }
        public string UserData { get; set; }

        public UserEventArgs(string login)
        {
            Login = login;
        }
    }
}
