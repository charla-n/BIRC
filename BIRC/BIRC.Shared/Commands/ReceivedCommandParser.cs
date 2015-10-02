using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BIRC.Shared.Models;

namespace BIRC.Shared.Commands
{
    public static class ReceivedCommandParser
    {
        private static string SEPARATOR = " ";

        private static Dictionary<string, Func<IEnumerable<string>, Connection, string>> Commands = new Dictionary<string, Func<IEnumerable<string>, Connection, string>>()
        {
            { "NOTICE", Notice },
            { "PING", Ping },
            { "001", RplWelcome },
            { "002", RplYourHost },
            { "003", RplCreated },
            { "004", RplMyInfo },
            { "005", RplSupport },
            { "251", RplLUserClient },
            { "254", RplLUserChannels },
            { "255", RplLUserMe },
            { "265", RplLocalUsers },
            { "266", RplGlobalUsers },
            { "422", ErrNoMotd },
            { "MODE", Mode },
        };

        public static string Parse(string cmd, string[] param, Connection c)
        {
            IEnumerable<string> noempty = param.Where(p => !string.IsNullOrEmpty(p));

            if (!Commands.ContainsKey(cmd))
                return null;
            return Commands[cmd].Invoke(noempty, c);
        }

        private static string Mode(IEnumerable<string> list, Connection c)
        {
            return string.Format(MainPage.GetString("Mode"), list.ToList()[1]);
        }

        private static string ErrNoMotd(IEnumerable<string> list, Connection c)
        {
            return list.ToList()[1];
        }

        private static string RplGlobalUsers(IEnumerable<string> list, Connection c)
        {
            list = list.Where(p => p != c.Nickname);

            return string.Join(SEPARATOR, list);
        }

        private static string RplLocalUsers(IEnumerable<string> list, Connection c)
        {
            list = list.Where(p => p != c.Nickname);

            return string.Join(SEPARATOR, list);
        }

        private static string RplLUserMe(IEnumerable<string> list, Connection c)
        {
            list = list.Where(p => p != c.Nickname);

            return string.Join(SEPARATOR, list);
        }

        private static string RplLUserChannels(IEnumerable<string> list, Connection c)
        {
            list = list.Where(p => p != c.Nickname);

            return string.Join(SEPARATOR, list);
        }

        private static string RplLUserClient(IEnumerable<string> list, Connection c)
        {
            list = list.Where(p => p != c.Nickname);

            return string.Join(SEPARATOR, list);
        }

        private static string RplSupport(IEnumerable<string> list, Connection c)
        {
            list = list.Where(p => p != c.Nickname);

            return string.Join(SEPARATOR, list);
        }

        private static string RplMyInfo(IEnumerable<string> list, Connection c)
        {
            List<string> cast = list.ToList();

            return string.Format(MainPage.GetString("RplMyInfo"), cast[1], cast[2], cast[3], cast[4]);
        }

        private static string RplCreated(IEnumerable<string> list, Connection c)
        {
            list = list.Where(p => p != c.Nickname);
            return string.Join(SEPARATOR, list);
        }

        private static string RplYourHost(IEnumerable<string> list, Connection c)
        {
            list = list.Where(p => p != c.Nickname);
            return string.Join(SEPARATOR, list);
        }

        private static string RplWelcome(IEnumerable<string> list, Connection c)
        {
            list = list.Where(p => p != c.Nickname);
            return string.Join(SEPARATOR, list);
        }

        private static string Ping(IEnumerable<string> list, Connection c)
        {
            return string.Format(MainPage.GetString("PingRec"), string.Join(SEPARATOR, list));
        }

        private static string Notice(IEnumerable<string> list, Connection c)
        {
            return string.Join(SEPARATOR, list);
        }
    }
}
