using BIRC.Shared.Exceptions;
using BIRC.Shared.Files;
using BIRC.Shared.Models;
using BIRC.Shared.Utils;
using IrcDotNet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BIRC.ViewModels;

namespace BIRC.Shared.Commands
{
    public static class PromptCommandParser
    {
        public static char[] SEPARATORS = { ' ' };
        public static char[] COMMA_SEPARATORS = { ',' };
        private static Dictionary<string, Action<IList<string>, AHistory>> Commands =
            new Dictionary<string, Action<IList<string>, AHistory>>()
        {
                { "/server", Server },
                { "/list", ListChan },
                { "/away", Away },
                { "/help", Help },
                { "/info", Info },
                { "/join", Join },
                { "/part", Part },
                { "/msg", Msg },
                { "/nick", Nick },
                { "/notice", Notice },
                { "/quit", Quit },
                { "/partall", PartAll },
                { "/ignore", Ignore },
                { "/unignore", UnIgnore },
                { "/whois", Whois },
                { "/whowas", Whowas },
                { "/who", Who },
                { "/invite", Invite },
                { "/mode", Mode },
                { "/kick", Kick },
                { "/op", Op },
                { "/deop", DeOp },
                { "/voice", Voice },
                { "/devoice", DeVoice },
        };

        public static void Parse(string cmd, AHistory c, List<string> p)
        {
            if (cmd.StartsWith("/") && !Commands.ContainsKey(cmd))
                throw new ErrorBIRC(string.Format(MainPage.GetErrorString("UnknownCommand"), cmd));
            else if (!cmd.StartsWith("/"))
                Message(cmd, c);
            else
                Commands[cmd].Invoke(p, c);
        }

        private static void DeVoice(IList<string> list, AHistory c)
        {
            if (list.Count < 2 || !(c is Channel))
                throw new ErrorBIRC(MainPage.GetErrorString("OpNoParam")); //TODO
            else
                c.Command.Mode(((Channel)c).RealName, "-v", list[1]);
        }

        private static void Voice(IList<string> list, AHistory c)
        {
            if (list.Count < 2 || !(c is Channel))
                throw new ErrorBIRC(MainPage.GetErrorString("OpNoParam")); //TODO
            else
                c.Command.Mode(((Channel)c).RealName, "+v", list[1]);
        }

        private static void Op(IList<string> list, AHistory c)
        {
            if (list.Count < 2 || !(c is Channel))
                throw new ErrorBIRC(MainPage.GetErrorString("OpNoParam")); //TODO
            else
                c.Command.Mode(((Channel)c).RealName, "+o", list[1]);
        }

        private static void DeOp(IList<string> list, AHistory c)
        {
            if (list.Count < 2 || !(c is Channel))
                throw new ErrorBIRC(MainPage.GetErrorString("OpNoParam")); //TODO
            else
                c.Command.Mode(((Channel)c).RealName, "-o", list[1]);
        }

        private static void Kick(IList<string> list, AHistory c)
        {
            if (list.Count < 3)
                throw new ErrorBIRC(MainPage.GetErrorString("KickNoParam")); //TODO
            else
                c.Command.Mode(list[1], list[2], list.Count == 3 ? "" : list[3]);
        }

        private static void Mode(IList<string> list, AHistory c)
        {
            if (list.Count < 3)
                throw new ErrorBIRC(MainPage.GetErrorString("ModeNoParam")); //TODO
            else
                c.Command.Mode(list[1], list[2], list.Count == 3 ? "" : list[3]);
        }

        private static void Invite(IList<string> list, AHistory c)
        {
            if (list.Count < 3)
                throw new ErrorBIRC(MainPage.GetErrorString("InviteNoParam")); //TODO
            else
                c.Command.Invite(list[1], list[2]);
        }

        private static void Who(IList<string> list, AHistory c)
        {
            if (list.Count < 2)
                c.Command.Who(null);
            else
                c.Command.Who(list[1]);
        }

        private static void Whowas(IList<string> list, AHistory c)
        {
            if (list.Count < 2)
                throw new ErrorBIRC(MainPage.GetErrorString("WhowasNoParam")); //TODO
            c.Command.Whowas(list[1].Split(COMMA_SEPARATORS));
        }

        private static void Whois(IList<string> list, AHistory c)
        {
            if (list.Count < 2)
                throw new ErrorBIRC(MainPage.GetErrorString("WhoisNoParam")); //TODO
            c.Command.Whois(list[1].Split(COMMA_SEPARATORS));
        }

        private static void Ignore(IList<string> list, AHistory c)
        {
            if (list.Count < 2)
                throw new ErrorBIRC(MainPage.GetErrorString("IgnoreNoParam")); //TODO
            c.Command.ChangeIgnoreState(list[1], true);
        }

        private static void UnIgnore(IList<string> list, AHistory c)
        {
            if (list.Count < 2)
                throw new ErrorBIRC(MainPage.GetErrorString("UnIgnoreNoParam")); //TODO
            c.Command.ChangeIgnoreState(list[1], false);
        }

        private static void Quit(IList<string> list, AHistory c)
        {
            c.Command.Disconnect();
        }

        private static void Notice(IList<string> list, AHistory c)
        {
            if (list.Count < 3)
                throw new ErrorBIRC(MainPage.GetErrorString("NoticeNoParam")); //TODO
            c.Command.Notice(list[1].Split(COMMA_SEPARATORS), string.Join(" ", list.Skip(2)));
        }

        private static void Nick(IList<string> list, AHistory c)
        {
            if (list.Count < 2)
                throw new ErrorBIRC(MainPage.GetErrorString("NickNoParam")); //TODO
            c.Command.Nick(list[1]);
        }

        private static void Msg(IList<string> list, AHistory c)
        {
            if (list.Count < 3)
                throw new ErrorBIRC(MainPage.GetErrorString("MsgNoParam")); //TODO
            IEnumerable<string> msg = list.Skip(2);

            c.Command.SendMessage(list[1], string.Join(" ", msg));
        }

        private static void Message(string msg, AHistory c)
        {
            Channel channel = c as Channel;

            if (channel != null && channel.RealName != null)
            {
                channel.ParentConnection.Command.SendMessage(channel.RealName, msg);
            }
            else if (channel != null && channel.RealName == null)
            {
                channel.ParentConnection.Command.SendMessage(channel.Name, msg);
            }
        }

        private static void PartAll(IList<string> list, AHistory c)
        {
            Connection co = c as Connection;
            Channel channel = c as Channel;

            if (co == null)
                co = channel.ParentConnection;
            c.Command.Part(co.Channels.Select(p => p.RealName));
        }

        private static void Part(IList<string> list, AHistory c)
        {
            if (list.Count == 1)
                throw new ErrorBIRC(MainPage.GetErrorString("PartNoParam")); // TODO
            if (list.Count == 2)
                c.Command.Part(list[1].Split(COMMA_SEPARATORS));
        }

        private static void Join(IList<string> list, AHistory c)
        {
            if (list.Count == 1)
                throw new ErrorBIRC(MainPage.GetErrorString("JoinNoParam"));
            if (list.Count == 2)
                c.Command.Join(list[1].Split(COMMA_SEPARATORS));
            else
            {
                List<Tuple<string, string>> result = new List<Tuple<string, string>>();
                string[] channels = list[1].Split(COMMA_SEPARATORS);
                string[] keys = list[2].Split(COMMA_SEPARATORS);

                for (int i = 0; i < channels.Length; i++)
                    result.Add(Tuple.Create(channels[i], keys.ElementAtOrDefault(i)));
                c.Command.Join(result);
            }
        }

        private static void Info(IList<string> list, AHistory c)
        {
            c.Command.Info();
        }

        private static void Help(IList<string> list, AHistory c)
        {
            if (list.Count == 1)
                c.Command.WriteToHistory(MainPage.GetInfoString("Help"));
            else
                c.Command.WriteToHistory(MainPage.GetInfoString("Help" + list[1]));
        }

        private static void Away(IList<string> list, AHistory c)
        {
            if (!c.Command.LocalUser.IsAway && list.Count == 1)
                throw new ErrorBIRC(MainPage.GetErrorString("AwayNotEnoughArg"));
            if (list.Count > 1)
                c.Command.Away(string.Join(ReceivedCommandParser.SEPARATOR, list.Skip(1)));
            else
                c.Command.Away();
        }

        private static void ListChan(IList<string> list, AHistory c)
        {
            IEnumerable<string> channels = list.Skip(1);

            c.Command.ListChannel(channels.Count() == 0 ? null : channels);
        }

        private async static void Server(IList<string> list, AHistory c)
        {
            string pwd = null;
            int port = 0;

            if (list.ElementAtOrDefault(1) == null)
                throw new ErrorBIRC(MainPage.GetErrorString("InvalidServerCmd"));
            if (list.ElementAtOrDefault(2) != null)
            {
                if (int.TryParse(list[2], out port) == false)
                    port = IrcClient.DefaultPort;
            }
            else
                port = IrcClient.DefaultPort;
            if (list.ElementAtOrDefault(3) != null)
                pwd = await Encryption.Protect(list[3]);
            Connection newc = new Connection()
            {
                AutoConnect = true,
                Name = list[1],
                Port = port,
                Password = pwd
            };
            AHistory cur = ConnectionUtils.Add(newc);
            ((BIRCViewModel)MainPage.currentDataContext).ServerSelection = cur;
            if (!cur.Command.IsConnected())
                cur.Command.Connect();
        }
    }
}
