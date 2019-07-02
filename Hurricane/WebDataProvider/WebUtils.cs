using System;

namespace Hurricane
{
    public class WebUtils
    {
        public static (string login, string password) GetCredentials()
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
