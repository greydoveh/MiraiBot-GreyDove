using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Data.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirai.Net.Utils.Scaffolds;
using Mirai.Net.Sessions.Http.Managers;

public class FriendForward {
    public FriendForward() {
        Bot.Instance.friendMessageEvent += Forward;
    }
    public async Task Forward(FriendMessageReceiver receiver) {
        if (receiver.FriendId == Admin.ID) return;
        receiver.MessageChain.ForEach(x => {
            if (x is FlashImageMessage) {
                x.Type = Messages.Image;
            }
        });
        await MessageManager.SendFriendMessageAsync(Admin.ID, $"[{DateTime.Now}]收到{receiver.FriendRemark}({receiver.FriendId})的消息：");
        await MessageManager.SendFriendMessageAsync(Admin.ID, receiver.MessageChain);
    }
}
