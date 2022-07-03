using AllModules;
using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Sessions;
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
    public Random rand;
    public event Func<GroupMessageReceiver, Task> groupMessageEvent;
    public event Func<FriendMessageReceiver, Task> friendMessageEvent;

    public static StreamReader sr;

    public Bot() {
        instance = this;
        bot = new MiraiBot {
            Address = "localhost:8080",
            QQ = "2978138785",
            VerifyKey = "INITKEYzFTDWZu0"
        };
        
        rand = new Random(DateTime.Now.Millisecond);

        sr = new StreamReader(@"D:\Code\C#\BotTest\BotTest\jrrp.txt");
        var jrrp = Program.jrrp;
        while (sr.Peek() != -1) {
            var t = sr.ReadLine().Split(' ');
            jrrp.Add(t[0], new KeyValuePair<DateTime, int>(new DateTime(long.Parse(t[1])), int.Parse(t[2])));
        }
        sr.Close();
        
        bot.MessageReceived.OfType<GroupMessageReceiver>().Subscribe(async receiver => await groupMessageEvent?.Invoke(receiver));
        bot.MessageReceived.OfType<FriendMessageReceiver>().Subscribe(async receiver => await friendMessageEvent?.Invoke(receiver));
    }

    public static implicit operator MiraiBot(Bot bot) {
        return bot.bot;
    }

    public async Task LaunchAsync() {
        await bot.LaunchAsync();
    }

    public void Dispose() {
        Console.WriteLine("Dispose");

        bot.Dispose();

        StreamWriter sw = new StreamWriter(@"D:\Code\C#\BotTest\BotTest\jrrp.txt");
        var jrrp = Program.jrrp;
        foreach (var rp in jrrp) {
            sw.WriteLine($"{rp.Key} {rp.Value.Key.Ticks} {rp.Value.Value}");
        }
        sw.Close();
    }
}
