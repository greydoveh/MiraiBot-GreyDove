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
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Events.Concretes.Request;
using Mirai.Net.Data.Shared;

public class Program
{
    public static async Task Main()
    {
        using Bot bot = new Bot();
        using GroupJrrp groupJrrp = new GroupJrrp();
        GroupFlash groupFlash = new GroupFlash();
        GroupRepeat groupRepeat = new GroupRepeat();
        GroupRecallBack groupRecallBack = new GroupRecallBack();
        FriendForward friendForward = new FriendForward();
        using KFCCrazyThursday kfc = new KFCCrazyThursday();
        Admin admin = new Admin();

        await bot.LaunchAsync();
        //await kfc.ThursdayReminder();

        Console.WriteLine("Bot start successfully!");

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
                    else if (bot.IsFriend(t[1]))
                        await MessageManager.SendFriendMessageAsync(t[1], t[2]);
                    else
                        Console.WriteLine("[error] 发送失败！不是好友！");
                    break;
                default:
                    break;
            }
        }
        
        async Task Test(string target = "1608369704") {
            if (bot.IsFriend(target))
                await MessageManager.SendFriendMessageAsync(target, new MessageChainBuilder().Face("187").Build());
        }
    }

}
