using Serilog;
using System;
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
        private readonly IDataProvider dataProvider;
        private readonly TelegramBotClient client;
        private HashSet<string> allowedUsers;

        public Bot(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
            client = new TelegramBotClient(Settings.API_TOKEN);  
            client.OnMessage += BotOnMessage;
        }

        public void Start()
        {
            allowedUsers = dataProvider.GetAllUsers();
            client.StartReceiving();
        }

        public void Stop()
        {
            client.StopReceiving();
        }

        private void BotOnMessage(object sender, MessageEventArgs e)
        {
            LogMessageFromChat(e.Message);

            if (IsMessageTypeAllowed(e.Message.Type))
            {
                string inMessage = e.Message.Text.ToLower();
                string outMessage;

                if (IsChatAllowed(e.Message.Chat.Id)
                    && IsLoginAllowed(inMessage))
                {
                    string userData = dataProvider.GetUserData(inMessage);
                    outMessage = userData;
                }
                else
                {
                    outMessage = string.Format("Hello {0}!", e.Message.Chat.FirstName);
                }
                SendMessageToChat(outMessage, e.Message.Chat.Id);

                Log.Information(string.Format("Respoce from bot:\r\n{0}", outMessage));
            }
        }

        private static void LogMessageFromChat(Message message)
        {
            Log.Information(string.Format("From: {0} {1}\r\nCaht ID: {2}\r\nMessage: {3}",
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
            return Settings.ALLOWED_CHATS.Contains(chatId);
        }

        private bool IsLoginAllowed(string login)
        {
            return allowedUsers.Contains(login);
        }

        private void SendMessageToChat(string message, long chatId)
        {
            client.SendTextMessageAsync(chatId, message);
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
