using Serilog;
using System;

namespace Hurricane
{
    class Program
    {
        static void Main(string[] args)
        {
            PrepareLogger();

            (string login, string password) = WebUtils.GetCredentials();
            IDataProvider dataProvider = new WebDataProvider(login, password);
            Bot bot = new Bot(dataProvider);

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
