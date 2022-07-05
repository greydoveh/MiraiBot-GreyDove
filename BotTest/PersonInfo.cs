using Mirai.Net.Data.Messages.Receivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PersonInfo {
    public string Id { get; set; }
    private KeyValuePair<DateTime, int> jrrp;
    public int Jrrp {
        get {
            if (jrrp.Key.Date != DateTime.Today) {
                jrrp = new KeyValuePair<DateTime, int>(DateTime.Today, Utility.random.Next(0, 101));
            } 
            return jrrp.Value;
        }
    }
    public PersonInfo(string QQNum) {
        Id = QQNum;
    }

    public PersonInfo(string QQNum, KeyValuePair<DateTime, int> rp) : this(QQNum) {
        jrrp = rp.Key.Date == DateTime.Today ? rp : new KeyValuePair<DateTime, int>(DateTime.Today, Utility.random.Next(0, 100));
    }

    public override string ToString() {
        return $"{Id} {jrrp.Key.Ticks} {jrrp.Value}";
    }

}
