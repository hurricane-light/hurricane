using Serilog;
using System;

namespace Hurricane
{
    class Program
    {
        private static IDataProvider dataProvider;
        
        static void Main(string[] args)
        {
            PrepareLogger();

            (string login, string password) = WebUtils.GetCredentials();

            Bot bot = new Bot();
            bot.OnMessage += BotOnMessage;

            dataProvider = new WebDataProvider(login, password);

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
            e.UserData = dataProvider.GetUserData(e.Login);
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
