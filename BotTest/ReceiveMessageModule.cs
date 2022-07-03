using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manganese.Text;
using Mirai.Net.Sessions;
using Mirai.Net.Data.Messages.Receivers;
using System.Reactive.Linq;
using Mirai.Net.Utils.Scaffolds;
using Mirai.Net.Sessions.Http.Managers;
using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Events;
using Mirai.Net.Data.Sessions;
using Mirai.Net.Data.Shared;
using Mirai.Net.Data.Exceptions;
using Mirai.Net.Data;
using Mirai.Net.Modules;
using Mirai.Net.Data.Messages.Concretes;

namespace AllModules;

public class ReceiveMessageModule : IModule {
    public bool? IsEnable { get; set; }

    public async void Execute(MessageReceiverBase @base) {
        if (@base is GroupMessageReceiver receiver) {
            GroupCmd(receiver);
        }
    }

    public async void GroupCmd(GroupMessageReceiver receiver) {
        //MessageChain cmds = receiver.MessageChain;

        List<MessageBase> cmds = new List<MessageBase>();
        foreach (var message in receiver.MessageChain) {
            if (message is PlainMessage plainMessage) {
                var t = plainMessage.Text.Split(' ');
                foreach (var tt in t) {
                    if (tt == "")   continue;
                    cmds.Add(new MessageChainBuilder().Plain(tt.Trim()).Build()[0]);
                }
            } else {
                cmds.Add(message);
            }
        }
        BotCmd botCmd = BotCmd.UnKnown;
        foreach (var cmd in cmds) {
            if (cmd is PlainMessage plain) {
                switch (plain) {
                    case ">test": botCmd = BotCmd.Test; break;
                    case ">mute": botCmd = BotCmd.Mute; break;
                    case ">unmute": botCmd = BotCmd.UnMute; break;
                    case ">recall": botCmd = BotCmd.ReCall; break;
                    case ">flash":  botCmd = BotCmd.Flash;  break;
                    case ">jrrp":  botCmd = BotCmd.JRRP;  break;
                    case ">h":  case ">help":   case ">?":
                        botCmd = BotCmd.Help; break;
                    default:    break;
                }
                if (botCmd != BotCmd.UnKnown)   break;
            }
        }
        switch (botCmd) {
            #region **TEST**
            case BotCmd.Test:
                await receiver.SendMessageAsync("THIS IS A TEST MESSAGE!");
                break;
            #endregion

            #region **禁言**
            case BotCmd.Mute:
                try {
                    if (cmds.Count == 4 && cmds[3] is PlainMessage) {
                        if (cmds[2] is AtMessage atMessage) {
                            await GroupManager.MuteAsync(atMessage.Target, receiver.GroupId, int.Parse(cmds[3] as PlainMessage) * 60);
                        } else if (cmds[2] is PlainMessage plainMessage) {
                            await GroupManager.MuteAsync(plainMessage, receiver.GroupId, int.Parse(cmds[3] as PlainMessage) * 60);
                        }  else {
                            await receiver.SendMessageAsync("Usage: >mute [At.] [Minute(s)]");
                        }
                        //await receiver.SendMessageAsync("禁言成功");
                    } else {
                        await receiver.SendMessageAsync("Usage: >mute [At.] [Minute(s)]");
                    }
                } catch (Exception) {
                    await receiver.SendMessageAsync("Usage: >mute [At.] [Minute(s)]");
                }
                break;
            #endregion

            #region **取消禁言**
            case BotCmd.UnMute:
                try {
                    if (cmds.Count == 3) {
                        if (cmds[2] is AtMessage atMessage) {
                            await GroupManager.UnMuteAsync(atMessage.Target, receiver.GroupId);
                        } else if (cmds[2] is PlainMessage plainMessage) {
                            await GroupManager.UnMuteAsync(plainMessage, receiver.GroupId);
                        }
                        //await receiver.SendMessageAsync("禁言成功");
                    } else {
                        await receiver.SendMessageAsync("Usage: >unmute [At. / QQ]");
                    }
                } catch (Exception) {
                    await receiver.SendMessageAsync("Usage: >unmute [At. / QQ]");
                }
                break;
            #endregion
                
            #region **帮助**
            case BotCmd.Help:
                await receiver.SendMessageAsync("******HELP****** \n>test 测试\n>mute 禁言 \n>numute 解除禁言\n>recall 撤回消息");
                break;
            #endregion

            #region **撤回**
            case BotCmd.ReCall:
                foreach (var cmd in cmds) {
                    if (cmd is QuoteMessage quote) {
                        await MessageManager.RecallAsync(quote.MessageId);
                        Console.WriteLine("撤回消息");
                        break;
                    }
                }
                break;
            #endregion

            #region **闪照**
            case BotCmd.Flash:
                if (Program.lastflash.ContainsKey(receiver.GroupId)) {
                    await receiver.SendMessageAsync(Program.lastflash[receiver.GroupId]);
                } else {
                    await receiver.SendMessageAsync("没有");
                }
                break;
            #endregion

            #region **闪照**
            case BotCmd.JRRP:
                Console.WriteLine(1);
                if (!Program.jrrp.ContainsKey(receiver.Sender.Id)){
                    Program.jrrp.Add(receiver.Sender.Id, new KeyValuePair<DateTime, int>(DateTime.Now.Date, Bot.Instance.rand.Next(0, 100)));
                    Console.WriteLine(DateTime.Now.Date);
                } else if (Program.jrrp[receiver.Sender.Id].Key.Date != DateTime.Now.Date) {
                    Console.WriteLine(DateTime.Now.Date);
                    Program.jrrp[receiver.Sender.Id] = new KeyValuePair<DateTime, int>(DateTime.Now.Date, Bot.Instance.rand.Next(0, 100));
                } 
                int jrrp = Program.jrrp[receiver.Sender.Id].Value;
                await receiver.SendMessageAsync(new MessageChainBuilder().
                        At(receiver.Sender.Id).
                        Plain($"您今日的幸运指数是{jrrp}/100（越低越好），为\"{CalcRP(jrrp)}\"").
                        Build());
                break;
            #endregion

            default:    break;
        }  
        
    }
    public static string CalcRP(int jrrp) {
        if (jrrp <= 25)    return "大吉";
        if (jrrp <= 50)    return "吉";
        if (jrrp <= 70)    return "末吉";
        if (jrrp <= 90)    return "凶";
        return "大凶";
    }
}

enum BotCmd {
    Test,
    Help,
    Mute,
    UnMute,
    ReCall,
    Flash,
    JRRP,
    UnKnown
}

