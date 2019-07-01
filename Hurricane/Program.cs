using Serilog;
using System;

namespace Hurricane
{
    class Program
    {
        private static Web web;
        static void Main(string[] args)
        {
            PrepareLogger();

            (string login, string password) = GetCredentials();

            Bot bot = new Bot("ApiToken");
            bot.OnMessage += BotOnMessage;

            web = new Web("https://epe.isd.dp.ua/isddb/db.aspx?mode=week&login=", login, password);

            try
            {
                bot.Start();
                Console.ReadLine();
            }
            catch(Exception e)
            {
                Log.Error(e, e.Message);
                throw e;
            }
            finally
            {
                bot.Stop();
            }
        }

        private static void BotOnMessage(object sender, UserEventArgs e)
        {
            string userDataHtmlPage = web.GetUserDataHtmlPage(e.Login);
            e.UserData = HtmlParser.GetUserData(userDataHtmlPage);
        }

        private static void PrepareLogger()
        {
            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .WriteTo.Console()
                        .WriteTo.File("logfile.log")
                        .CreateLogger();
        }

        private static (string login, string password) GetCredentials()
        {
            Console.Write("login: ");
            string login = Console.ReadLine();
            Console.Write("password: ");
            string password = ReadPassword();
            Console.Clear();

            return (login, password);
        }

        private static string ReadPassword()
        {
            string password = string.Empty;

            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);

            return password;
        }
    }
}
