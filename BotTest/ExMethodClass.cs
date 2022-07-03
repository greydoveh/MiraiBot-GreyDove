using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Utils.Scaffolds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class ExMethodClass {
    public static MessageChainBuilder Face(this MessageChainBuilder builder, string FaceId) {
        return builder.Append(new FaceMessage { FaceId = FaceId, Name = "" });
    }
    [Obsolete("复读机 后续只复读文本好了")]
    public static bool Equal (this MessageChain a, MessageChain b) {
        if (a.Count != b.Count) {
            return false;
        }
        bool f = true;
        for (int i = 0; i < a.Count; i++) {
            if (a[i] is SourceMessage)  continue;

            if (a[i] is ImageMessage aImage) {
                if (!(b[i] is ImageMessage) || aImage.ImageId != (b[i] as ImageMessage).ImageId) {
                    f = false;   break;
                }
            } else if (a[i] is PlainMessage aPlain) {
                if (!(b[i] is PlainMessage) || aPlain.Text != (b[i] as PlainMessage).Text) {
                    f = false;   break;
                }
            }
        }
        return f;
    }
}