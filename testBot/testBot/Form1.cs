using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace testBot
{
    public partial class Form1 : Form
    {
        private static readonly string botToken = "343393266:AAG1SltZt3VaZNydfqNw4-i0p8w0DcXKlBo";
        private static List<MessageEntityType> entityType = new List<MessageEntityType>(new MessageEntityType[] { MessageEntityType.Url, MessageEntityType.Mention, MessageEntityType.TextLink, MessageEntityType.TextMention });
        private static List<string> entityGuess = new List<string>(new string[] { "@", "www", "http", ".com", ".me", ".net", ".co", ".uk", ".org" });
        private static readonly TelegramBotClient bot = new TelegramBotClient(botToken);
        public Form1()
        {
            InitializeComponent();
            bot.OnMessage += botOnMessageReceived;
            bot.OnMessageEdited += botOnMessageReceived;
            bot.OnReceiveError += botOnErrorReceived;
            bot.StartReceiving();
        }
        private static void botOnErrorReceived(object sender, ReceiveErrorEventArgs e)
        {
            Debugger.Break();
        }

        private static async void botOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            var gChat = await bot.GetChatAsync(message.Chat.Id);
            if(message.Text != null)
            {
                WebRequest req = WebRequest.Create("https://api.telegram.org/bot" + botToken + "/deleteMessage?chat_id=" + gChat.Id + "&message_id=" + message.MessageId);
                req.UseDefaultCredentials = true;
                var result = await req.GetResponseAsync();
                req.Abort();
            }
            if (message.Text != null && message.Entities != null)
            {
                foreach (var entity in message.Entities)
                {
                    if (entityType.Contains(entity.Type))
                    {
                        WebRequest req = WebRequest.Create("https://api.telegram.org/bot" + botToken + "/deleteMessage?chat_id=" + gChat.Id + "&message_id=" + message.MessageId);
                        req.UseDefaultCredentials = true;
                        var result = await req.GetResponseAsync();
                        req.Abort();
                    }
                }
            }
            else if (message.Caption != null)
            {
                foreach (string srch in entityGuess)
                {
                    if (message.Caption.Contains(srch) == true && message.Caption.Contains("@yahoo") == false && message.Caption.Contains("@gmail") == false)
                    {
                        WebRequest req = WebRequest.Create("https://api.telegram.org/bot" + botToken + "/deleteMessage?chat_id=" + gChat.Id + "&message_id=" + message.MessageId);
                        req.UseDefaultCredentials = true;
                        var result = await req.GetResponseAsync();
                        req.Abort();
                    }
                }
            }

            else if (message.Sticker != null)
            {
                WebRequest req = WebRequest.Create("https://api.telegram.org/bot" + botToken + "/deleteMessage?chat_id=" + gChat.Id + "&message_id=" + message.MessageId);
                req.UseDefaultCredentials = true;
                var result = await req.GetResponseAsync();
                req.Abort();
            }

        }
    }


}




