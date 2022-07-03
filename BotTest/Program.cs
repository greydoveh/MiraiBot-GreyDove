// See https://aka.ms/new-console-template for more information
using Manganese.Text;
using Mirai.Net.Sessions;
using Mirai.Net.Data.Messages.Receivers;
using System.Linq;
using System;
using System.IO;
using System.Reactive.Linq;
using Mirai.Net.Utils.Scaffolds;
using Mirai.Net.Sessions.Http.Managers;
using Mirai.Net.Data.Messages;
using Mirai.Net.Utils;
using AllModules;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Events.Concretes.Request;
using Mirai.Net.Data.Shared;

public class Program
{
    private static Program instance;
    public static MiraiBot bot;
    public static Dictionary<string, MessageBase> lastflash;
    public static ReceiveMessageModule receiveModule;
    public static Dictionary<string, KeyValuePair<DateTime, int>> jrrp;
    public static StreamReader sr;

    static Program () {
        
        lastflash = new Dictionary<string, MessageBase>();
        
        sr = new StreamReader(@"D:\Code\C#\BotTest\BotTest\jrrp.txt");
        var jrrp = Program.jrrp;
        while (sr.Peek() != -1) {
            var t = sr.ReadLine().Split(' ');
            jrrp.Add(t[0], new KeyValuePair<DateTime, int>(new DateTime(long.Parse(t[1])), int.Parse(t[2])));
        }
        sr.Close();
        bot = new MiraiBot {
            Address = "localhost:8080",
            QQ = "2978138785",
            VerifyKey = "INITKEYzFTDWZu0"
        };
        receiveModule = new ReceiveMessageModule();
        jrrp = new Dictionary<string, KeyValuePair<DateTime, int>>();
        
        
    }

    public static async Task Main()
    {
        //using Bot bot = new Bot();
        Dictionary<string, MessageChain> lastMessage = new Dictionary<string, MessageChain>();
        Dictionary<string, bool> repeat = new Dictionary<string, bool>();
        //bool repeat = false;
        #region **群消息**
        bot.MessageReceived.OfType<GroupMessageReceiver>()
            .Subscribe(async receiver => {
                //if (receiver.GroupId != "582491451")   return ;
                Console.WriteLine($"Group:\n" + receiver.MessageChain.ToJsonString());
                if (receiver.MessageChain.GetPlainMessage() == "yanyan") {
                    await receiver.SendMessageAsync(new MessageChainBuilder().Face("187").Build());
                    return;
                }
                #region **获取闪照**
                foreach (var message in receiver.MessageChain) {
                    if (message is FlashImageMessage flash) {
                        if (lastflash.ContainsKey(receiver.GroupId))
                            lastflash[receiver.GroupId] = message;
                        else
                            lastflash.Add(receiver.GroupId, message);
                        lastflash[receiver.GroupId].Type = Messages.Image;
                    }
                }
                #endregion

                if (receiver.MessageChain.Any(x => x is PlainMessage)
                && receiver.MessageChain.GetPlainMessage().StartsWith(">")) {
                    receiveModule.Execute(receiver);

                    if (!lastMessage.ContainsKey(receiver.GroupId)) {
                        lastMessage.Add(receiver.GroupId, receiver.MessageChain);
                        repeat.Add(receiver.GroupId, false);
                    } else if (!lastMessage[receiver.GroupId].Equal(receiver.MessageChain)) {
                        lastMessage[receiver.GroupId] = receiver.MessageChain;
                        repeat[receiver.GroupId] = false;
                    }
                    return;
                }

                #region **复读机**
                if (lastMessage.ContainsKey(receiver.GroupId))
                    Console.WriteLine("DEBUG REPEAT " + repeat + "\n" + receiver.MessageChain.ToJsonString() + "\n" + lastMessage[receiver.GroupId].ToJsonString());

                if ((!repeat.ContainsKey(receiver.GroupId) || !repeat[receiver.GroupId])
                && lastMessage.ContainsKey(receiver.GroupId)
                && lastMessage[receiver.GroupId].Equal(receiver.MessageChain)) {
                    await receiver.SendMessageAsync(receiver.MessageChain);
                    repeat[receiver.GroupId] = true;
                    return;
                }
                if (!lastMessage.ContainsKey(receiver.GroupId)) {
                    lastMessage.Add(receiver.GroupId, receiver.MessageChain);
                    repeat.Add(receiver.GroupId, false);
                } else if (!lastMessage[receiver.GroupId].Equal(receiver.MessageChain)) {
                    lastMessage[receiver.GroupId] = receiver.MessageChain;
                    repeat[receiver.GroupId] = false;
                }
                #endregion

                Console.WriteLine($"[{receiver.GroupName}({receiver.GroupId})] " +
                    $"{receiver.Sender.Name}({receiver.Sender.Id}): {receiver.MessageChain.GetPlainMessage()}");
            });
        #endregion

        #region **好友消息**
        bot.MessageReceived.OfType<FriendMessageReceiver>()
            .Subscribe(async receiver => {
                Console.WriteLine($"Friend:\n" + receiver.MessageChain.ToJsonString());

                if (receiver.FriendId != "1608369704") {
                    var message = receiver.MessageChain;
                    await MessageManager.SendFriendMessageAsync("1608369704",
                        $"[{DateTime.Now}]{receiver.FriendName}({receiver.FriendId}):");
                    for (int i = 0; i < message.Count; i++) {
                        if (message[i].Type == Messages.FlashImage) {
                            message[i].Type = Messages.Image;
                        }
                    }
                    await MessageManager.SendFriendMessageAsync("1608369704", receiver.MessageChain);
                }

                Console.WriteLine($"{receiver.FriendName}({receiver.FriendId}): {receiver.MessageChain.GetPlainMessage()}");
                await receiver.SendMessageAsync("收到");
            });
        #endregion
        
        #region **拒绝加群**
        bot.EventReceived.OfType<NewMemberRequestedEvent>()
            .Subscribe(async e => {
                ////同意入群
                //await RequestManager.HandleNewMemberRequestedAsync(e, NewMemberRequestHandlers.Approve);
                ////或者
                //await e.Handle(NewMemberRequestHandlers.Approve);

                ////拒绝入群
                //await RequestManager.HandleNewMemberRequestedAsync(e, NewMemberRequestHandlers.Reject, "不接受的原因(可选，仅在拒绝请求时有效)");
                //或者
                await e.Handle(NewMemberRequestHandlers.Reject, "拒绝入群");
            });
        #endregion
        
        //注意: `LaunchAsync`是一个异步方法，请确保`Main`方法的返回值为`Task`
        await bot.LaunchAsync();

        Console.WriteLine("Bot start successfully!");

        #region **控制台**
        while (true) {
            string cmd = Console.ReadLine().ToLower();
            if (cmd == "exit")  break;
            string[] t = cmd.Split(' ');
            switch (t[0]) {
                case "test":
                    await Test();
                    break;
                case "send":
                    if (t[1] == "group") 
                        await MessageManager.SendGroupMessageAsync(t[2], t[3]);
                    else if (MiraiScaffold.IsFriend(bot, t[1]))
                        await MessageManager.SendFriendMessageAsync(t[1], t[2]);
                    else
                        Console.WriteLine("[error] 发送失败！不是好友！");
                    break;
                default:
                    break;
            }
        }
        #endregion
        
        Console.WriteLine("Dispose");

        StreamWriter sw = new StreamWriter(@"D:\Code\C#\BotTest\BotTest\jrrp.txt");
        foreach (var rp in jrrp) {
            sw.WriteLine($"{rp.Key} {rp.Value.Key.Ticks} {rp.Value.Value}");
        }
        sw.Close();

        bot.Dispose();

        async Task Test(string target = "1608369704") {
            if (MiraiScaffold.IsFriend(bot, target))
                await MessageManager.SendFriendMessageAsync(target, new MessageChainBuilder().Face("187").Build());
        }
    }

}
