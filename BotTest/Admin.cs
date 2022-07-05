using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Utils.Scaffolds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Admin : PersonInfo {
    public const string ID = "1608369704";
    public Admin() : base("1608369704") {
        Bot.Instance.friendMessageEvent += Test;
    }
    public async Task Test(FriendMessageReceiver receiver) {
        Console.WriteLine("test");
    }
}
