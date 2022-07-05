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

    public static bool Equal (this MessageChain a, MessageChain b) {
        if (a == null && b == null) {
            return true;
        }
        if (a == null || b == null) {
            return false;
        }
        if (a.Count != b.Count) {
            return false;
        }
        for (int i = 0; i < a.Count; i++) {
            if (a[i] is SourceMessage)  continue;

            if (a[i] != b[i]) {
                return false;
            }
        }
        return true;
    }
}