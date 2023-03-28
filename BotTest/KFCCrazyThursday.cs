using Mirai.Net.Sessions.Http.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KFCCrazyThursday : IDisposable {
    DateTime dt;
    public KFCCrazyThursday() { 
        StreamReader sr = new StreamReader(@"D:\Code\C#\BotTest\BotTest\Thursday.txt");    
        string line = sr.ReadLine();
        sr.Close();
        dt = new DateTime(long.Parse(line));
    }

    public async Task ThursdayReminder() {
        if (dt != DateTime.Today && DateTime.Now.DayOfWeek == DayOfWeek.Thursday) {
            await MessageManager.SendGroupMessageAsync("137328563", 
                "正在播放：\n妈的群主不请我吃肯德基疯狂星期四\n━━━━━━━●──3:42 \n ⇆    ◁    ❚❚    ▷     ↻");
            dt = DateTime.Today;
        }
    }

    public void Dispose() {
        StreamWriter sw = new StreamWriter(@"D:\Code\C#\BotTest\BotTest\Thursday.txt"); 
        sw.WriteLine($"{dt.Ticks}");
        sw.Close();
    }
}
