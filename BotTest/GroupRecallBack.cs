using Mirai.Net.Data.Events.Concretes.Group;
using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Sessions.Http.Managers;
using Mirai.Net.Utils.Scaffolds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GroupRecallBack {

    public GroupRecallBack() {
        Bot.Instance.groupRecallEvent += Recall;
        Bot.Instance.groupMessageEvent += Back;
    }
    
    public async Task Recall(GroupMessageRecalledEvent e) {
        var group = GroupInfo.GetGroupInfo(e.Group.Id);
        var t = group.messages.Find(x => x.Any(y => y is SourceMessage && (y as SourceMessage).MessageId == e.MessageId));
        group.RecallMessageChain = t;
    }

    public async Task Back(GroupMessageReceiver receiver) {
        if (receiver.MessageChain.GetPlainMessage() != ">back")    return;
        var group = GroupInfo.GetGroupInfo(receiver.GroupId);
        if (group.RecallMessageChain == null) return;
        await receiver.SendMessageAsync(group.RecallMessageChain);
    }
}
