using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
        private static readonly string botToken = "343393266:AAH8BBukaPDTYjou5IOgrthO706oQkzlxI8";
        private static List<MessageEntityType> entityType = new List<MessageEntityType>(new MessageEntityType[] { MessageEntityType.Url, MessageEntityType.Mention, MessageEntityType.TextLink, MessageEntityType.TextMention });
        private static List<string> entityGuess = new List<string>(new string[] { "@", "www", "http", ".com", ".me", ".net", ".co", ".uk", ".org" });
        private static readonly TelegramBotClient bot = new TelegramBotClient(botToken);

        //kgk: -1001088853897
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
            #region Group Management
            try
            {
                if (message.Chat.Type == ChatType.Group || message.Chat.Type == ChatType.Supergroup)
                {
                    var getChatMember = await bot.GetChatMemberAsync(message.Chat.Id, message.From.Id);
                    if (getChatMember.Status == ChatMemberStatus.Member)
                    {
                        if (message.ForwardFrom != null || message.ForwardFromChat != null)
                        {
                            await DeleteMessageAsync(message.Chat.Id, message.MessageId);
                        }
                        switch (message.Type)
                        {
                            case MessageType.TextMessage:
                                if (message.Entities != null)
                                {
                                    foreach (var entity in message.Entities)
                                    {
                                        if (entityType.Contains(entity.Type))
                                        {
                                            await DeleteMessageAsync(message.Chat.Id, message.MessageId);
                                        }
                                    }
                                }
                                break;
                            default:
                                await DeleteMessageAsync(message.Chat.Id, message.MessageId);
                                break;
                        }
                    }
                    #endregion Group Management
                }
            }
            catch
            {

            }

        }
        private static async Task DeleteMessageAsync(long chat_id, int message_id)
        {
            try
            {
                using (var client = new WebClient())
                {
                    var content = await client.DownloadStringTaskAsync("https://api.telegram.org/bot" + botToken + "/deleteMessage?chat_id=" + chat_id + "&message_id=" + message_id);
                }
            }
            catch
            {

            }
        }
    }


}




