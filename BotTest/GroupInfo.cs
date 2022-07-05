using Mirai.Net.Data.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GroupInfo {
    public static List<GroupInfo> list = new List<GroupInfo>();
    public string Id { get; set; }
    public string Name { get; set; }
    
    public MessageChain LastFlashImage { get; set; }
    public MessageChain LastMessageChain { get; set;}
    public bool CanRepeat { get; set;}

    public static GroupInfo GetGroupInfo (string num) {
        var group = list.Find(x => x.Id == num);
        if (group == null) {
            list.Add(new GroupInfo(num));
            group = list.Last();
        }
        return group;
    }


    public GroupInfo(string QQGroupNum, string name = "") {
        Id = QQGroupNum;
        Name = name;
    }

    public static implicit operator string(GroupInfo groupInfo) => groupInfo.Id;
    public static implicit operator GroupInfo(string QQnum) => new GroupInfo(QQnum);
}
