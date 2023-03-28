using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Sessions.Http.Managers;
using Mirai.Net.Utils.Scaffolds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Admin : PersonInfo {
    public const string ID = "1608369704";
    public Admin() : base(ID) {
        Bot.Instance.friendMessageEvent += SendMessage;
    }
    public async Task SendMessage(FriendMessageReceiver receiver) {
        if (receiver.FriendId != ID)    return ;
        string s = receiver.MessageChain.GetPlainMessage();
        string[] t = s.Split(' ');
        if (t.Length < 3) return ;
        string message = "";
        for (int i = 2; i < t.Length; i++) {
            message += t[i] + " ";
        }
        if (t[0] == ">sendgroup") {
            await MessageManager.SendGroupMessageAsync(t[1], message);
        } else if (t[0] == ">send") {
            await MessageManager.SendFriendMessageAsync(t[1], message);
        }
    }
}
