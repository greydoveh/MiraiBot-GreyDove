using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Sessions;
using Mirai.Net.Utils.Scaffolds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

public class Bot : IDisposable {
    private static Bot instance;
    public static Bot Instance { 
        get {
            if (instance == null) {
                instance = new Bot();
            }
            return instance;
        } 
    }
    private MiraiBot bot;
    public event Func<GroupMessageReceiver, Task> groupMessageEvent;
    public event Func<FriendMessageReceiver, Task> friendMessageEvent;

    public Bot() {
        instance = this;
        bot = new MiraiBot {
            Address = "localhost:8080",
            QQ = "2978138785",
            VerifyKey = "INITKEYzFTDWZu0"
        };

        
        bot.MessageReceived.OfType<GroupMessageReceiver>()
            .Subscribe(async receiver => await groupMessageEvent?.Invoke(receiver));
        bot.MessageReceived.OfType<FriendMessageReceiver>()
            .Subscribe(async receiver => await friendMessageEvent?.Invoke(receiver));
    }

    public static implicit operator MiraiBot(Bot bot) => bot.bot;
    public async Task LaunchAsync() => await bot.LaunchAsync();
    public bool IsFriend(string qq) => MiraiScaffold.IsFriend(bot, qq);

    public void Dispose() {
        Console.WriteLine("Dispose");

        bot.Dispose();
    }
}
