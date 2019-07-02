using Serilog;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Hurricane
{
    public class Bot
    {
        private IDataProvider dataProvider;
        private readonly TelegramBotClient client;
        private readonly HashSet<long> allowedChats = new HashSet<long>
        {
            248580711,
            438265830,
            110638968,
            98331770
        };

        private readonly HashSet<string> allowedLogins = new HashSet<string>
        {
            "psok",
            "tsok",
            "drud",
            "ovor"
        };

        public Bot(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
            client = new TelegramBotClient(Settings.API_TOKEN);
            client.OnMessage += BotOnMessage;
        }

        public void Start()
        {
            client.StartReceiving();
        }

        public void Stop()
        {
            client.StopReceiving();
        }

        private void BotOnMessage(object sender, MessageEventArgs e)
        {
            LogMessage(e.Message);

            if (IsMessageTypeAllowed(e.Message.Type))
            {
                if (IsChatAllowed(e.Message.Chat.Id)
                    && IsLoginAllowed(e.Message.Text))
                {
                    string login = e.Message.Text;
                    string userData = dataProvider.GetUserData(login);
                    client.SendTextMessageAsync(e.Message.Chat.Id, userData);
                }
                else
                {
                    client.SendTextMessageAsync(e.Message.Chat.Id,
                                                string.Format("Hello {0}!", e.Message.Chat.FirstName));
                }
            }
        }

        private static void LogMessage(Message message)
        {
            Log.Information(string.Format("Message from: {0} {1}\r\nCaht ID: {2}\r\nMessage: {3}",
                            message.Chat.FirstName, message.Chat.LastName,
                            message.Chat.Id,
                            message.Text));
        }

        private bool IsMessageTypeAllowed(MessageType type)
        {
            return MessageType.Text == type;
        }

        private bool IsChatAllowed(long chatId)
        {
            return allowedChats.Contains(chatId);
        }

        private bool IsLoginAllowed(string login)
        {
            return allowedLogins.Contains(login);
        }

        private bool IsLooksLikeLogin(string text)
        {
            if (text.Length > 4)
            {
                return false;
            }

            Regex regex = new Regex("[a-z]*");
            MatchCollection matches = regex.Matches(text);

            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    if (match.Value.Length == 4)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
