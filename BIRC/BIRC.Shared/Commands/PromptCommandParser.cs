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
        };

        public static void Parse(string cmd, AHistory c, List<string> p)
        {
            if (!Commands.ContainsKey(cmd))
                throw new ErrorBIRC(string.Format(MainPage.GetErrorString("UnknownCommand"), cmd));
            Commands[cmd].Invoke(p, c);
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
                Server = new Models.Server()
                {
                    Name = list[1],
                    Port = port,
                },
                Password = pwd
            };
            AHistory cur = ConnectionUtils.Add(newc);
            ((BIRCViewModel)MainPage.currentDataContext).ServerSelection = cur;
            if (!cur.Command.IsConnected())
                cur.Command.Connect();
        }
    }
}
