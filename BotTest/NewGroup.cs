using Mirai.Net.Data.Events.Concretes.Request;
using Mirai.Net.Data.Shared;
using Mirai.Net.Sessions.Http.Managers;
using Mirai.Net.Utils.Scaffolds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NewGroup {
    public NewGroup() {
        Bot.Instance.newGroupRequestEvent += Request;
    }
    public async Task Request(NewMemberRequestedEvent e) {
        if (e.FromId == Admin.ID) {
            await RequestManager.HandleNewMemberRequestedAsync(e, NewMemberRequestHandlers.Approve);
        } else {
            await RequestManager.HandleNewMemberRequestedAsync(e, NewMemberRequestHandlers.Reject);
        }
    }
}
