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
        private static readonly string botToken = "343393266:AAF1k2lXAai73noGmxuVRyW9f-JeD7s25MQ";
        private static List<MessageEntityType> entityType = new List<MessageEntityType>(new MessageEntityType[] { MessageEntityType.Url, MessageEntityType.Mention, MessageEntityType.TextLink, MessageEntityType.TextMention });
        private static List<string> entityGuess = new List<string>(new string[] { "@","www", "http", ".com", ".me",".net",".co",".uk", ".org"});
        private static WebClient webClient = new WebClient();
        private static string address, result;
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
            try
            {
                var message = messageEventArgs.Message;
                if(message.Text != null && message.Entities != null)
                {
                    foreach (var entity in message.Entities)
                    {
                        if (entityType.Contains(entity.Type))
                        {
                            address = "https://api.telegram.org/bot" + botToken + "/deleteMessage?chat_id=" + message.Chat.Id + "&message_id=" + message.MessageId;
                            result = await webClient.DownloadStringTaskAsync(address);
                        }
                    }
                }
                else if(message.Caption != null)
                {
                    foreach (string srch in entityGuess)
                    {
                        if (message.Caption.Contains(srch) == true && message.Caption.Contains("@yahoo") == false && message.Caption.Contains("@gmail") == false)
                        {
                            address = "https://api.telegram.org/bot" + botToken + "/deleteMessage?chat_id=" + message.Chat.Id + "&message_id=" + message.MessageId;
                            result = await webClient.DownloadStringTaskAsync(address);
                        }
                    }
                }

                else if (message.Sticker != null)
                {
                    address = "https://api.telegram.org/bot" + botToken + "/deleteMessage?chat_id=" + message.Chat.Id + "&message_id=" + message.MessageId;
                    result = await webClient.DownloadStringTaskAsync(address);
                }

            }
            catch (WebException)
            {
               
            }
        }
    }


}




