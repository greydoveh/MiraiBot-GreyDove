using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Utils.Scaffolds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GroupRepeat {
    public GroupRepeat() {
        Bot.Instance.groupMessageEvent += Repeat;
    }

    public async Task Repeat(GroupMessageReceiver receiver) {
        var group = GroupInfo.GetGroupInfo(receiver.GroupId);

        if (group.CanRepeat && group.LastMessageChain.Equal(receiver.MessageChain)) {
            await receiver.SendMessageAsync(receiver.MessageChain);
            group.CanRepeat = false;
        } else if (!group.LastMessageChain.Equal(receiver.MessageChain)) {
            group.LastMessageChain = receiver.MessageChain;
            group.CanRepeat = true;
        }
    }
}
