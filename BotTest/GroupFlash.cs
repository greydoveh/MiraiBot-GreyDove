using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Utils.Scaffolds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GroupFlash {

    public GroupFlash() {
        Bot.Instance.groupMessageEvent += Flash;
    }

    public async Task Flash(GroupMessageReceiver receiver) {
        var group = GroupInfo.GetGroupInfo(receiver.GroupId);
        
        if (receiver.MessageChain.Any(x => x is FlashImageMessage)) {
            group.LastFlashImage = new MessageChain(receiver.MessageChain);
            group.LastFlashImage.ForEach(x => x.Type = Messages.Image);
        } else if (receiver.MessageChain.GetPlainMessage() == ">flash" && group.LastFlashImage != null) {
            await receiver.SendMessageAsync(group.LastFlashImage).RecallAfter(TimeSpan.FromSeconds(5));
        }
    }

}
