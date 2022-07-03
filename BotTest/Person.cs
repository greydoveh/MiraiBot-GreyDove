using Mirai.Net.Data.Messages.Receivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Person {
    public string Id { get; set; }
    public Person(string QQNum) { 
        Id = QQNum;
        //Bot.Instance.friendMessageEvent += Fun;
    }

    //public async Task Fun(FriendMessageReceiver receiver) {
        
    //}
}
