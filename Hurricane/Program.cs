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

            (string login, string password) = WebUtils.GetCredentials();

            Bot bot = new Bot();
            bot.OnMessage += BotOnMessage;

            web = new Web(login, password);

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
    }
}
