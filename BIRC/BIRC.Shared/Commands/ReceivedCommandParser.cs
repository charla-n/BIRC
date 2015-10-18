using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BIRC.Shared.Models;

namespace BIRC.Shared.Commands
{
    public static class ReceivedCommandParser
    {
        public static string IGNORE = "BIRC_IGNORE";
        public static string SEPARATOR = " ";

        private static Dictionary<string, Func<IEnumerable<string>, Connection, string>> Commands = new Dictionary<string, Func<IEnumerable<string>, Connection, string>>()
        {
            { "NOTICE", Notice },
            { "PING", Ping },
            { "001", GetInfo },
            { "002", GetInfo },
            { "003", GetInfo },
            { "004", RplMyInfo },
            { "005", GetInfo },
            { "251", GetInfo },
            { "254", GetInfo },
            { "255", GetInfo },
            { "265", GetInfo },
            { "266", GetInfo },
            { "422", Ignore },
            { "MODE", Mode },
            { "321", Ignore },
            { "323", Ignore },
            { "322", GetInfo },
            { "306", GetInfo },
            { "305", GetInfo },
            { "351", Ignore },
            { "391", Ignore },
            { "JOIN", Ignore },
            { "353", Ignore },
            { "366", Ignore },
            { "PRIVMSG", Ignore },
            { "QUIT", Ignore }
        };

        public static string Parse(string cmd, string[] param, Connection c)
        {
            IEnumerable<string> noempty = param.Where(p => !string.IsNullOrEmpty(p));

            if (!Commands.ContainsKey(cmd))
                return null;
            return Commands[cmd].Invoke(noempty, c);
        }

        private static string GetInfo(IEnumerable<string> list, Connection c)
        {
            list = list.Where(p => p != c.Nickname);

            return string.Join(SEPARATOR, list);
        }

        private static string Ignore(IEnumerable<string> list, Connection c)
        {
            return IGNORE;
        }

        private static string Mode(IEnumerable<string> list, Connection c)
        {
            return string.Format(MainPage.GetInfoString("Mode"), list.ToList()[1]);
        }

        private static string RplMyInfo(IEnumerable<string> list, Connection c)
        {
            List<string> cast = list.ToList();

            return string.Format(MainPage.GetInfoString("RplMyInfo"), cast[1], cast[2], cast[3], cast[4]);
        }

        private static string Ping(IEnumerable<string> list, Connection c)
        {
            return string.Format(MainPage.GetInfoString("PingRec"), string.Join(SEPARATOR, list));
        }

        private static string Notice(IEnumerable<string> list, Connection c)
        {
            return string.Join(SEPARATOR, list);
        }
    }
}
