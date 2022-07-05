using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Utils.Scaffolds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GroupJrrp : IDisposable {
    private List<PersonInfo> personInfos;

    public GroupJrrp() {
        personInfos = new List<PersonInfo>();
        
        StreamReader sr = new StreamReader(@"D:\Code\C#\BotTest\BotTest\jrrp.txt");
        while (sr.Peek() != -1) {
            var t = sr.ReadLine().Split(' ');
            personInfos.Add(new PersonInfo(t[0], 
                new KeyValuePair<DateTime, int>(new DateTime(long.Parse(t[1])), int.Parse(t[2]))));
        }
        sr.Close();

        Bot.Instance.groupMessageEvent += Jrrp;
    }

    public async Task Jrrp(GroupMessageReceiver receiver) {
        if (receiver.MessageChain.GetPlainMessage() != ">jrrp")    return;
        if (!personInfos.Any(x => x.Id == receiver.Sender.Id)) {
            personInfos.Add(new PersonInfo(receiver.Sender.Id));
        }
        var t = personInfos.Find(x => x.Id == receiver.Sender.Id);
        MessageChain message = new MessageChainBuilder()
            .At(receiver.Sender)
            .Plain($" 您今日的幸运指数是{t.Jrrp}/100（越低越好），为\"{CalcRP(t.Jrrp)}\"")
            .Build();
        await receiver.SendMessageAsync(message);
    }

    public static string CalcRP(int jrrp) {
        if (jrrp <= 25)    return "大吉";
        if (jrrp <= 50)    return "吉";
        if (jrrp <= 70)    return "末吉";
        if (jrrp <= 90)    return "凶";
        return "大凶";
    }

    public void Dispose() {
        StreamWriter sw = new StreamWriter(@"D:\Code\C#\BotTest\BotTest\jrrp.txt");
        foreach (var p in personInfos) {
            sw.WriteLine(p);
        }
        sw.Close();
    }
}
