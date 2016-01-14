using BIRC.Shared.Files;
using BIRC.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;
using BIRC.ViewModels;
using IrcDotNet;

namespace BIRC.Shared.Utils
{
    public static class ConnectionUtils
    {
        public static Connection Add(Connection c)
        {
            Connection get = (Connection)ConnectionFile.Instance().Connections.FirstOrDefault(
                p => p is Connection && ((Connection)p).Name == c.Name);
            if (get != null)
                return get;
            ConnectionFile.Instance().Connections.Add(c);
            ConnectionFile.Instance().WriteImpl(ConnectionFile.Instance().Connections.Where(p => p is Connection).Cast<Connection>().ToList());
            return c;
        }

        public static void AddChannel(Channel channel)
        {
            if (ConnectionFile.Instance().Connections.FirstOrDefault(p => (p is Channel && ((Channel)p).Name == channel.Name)
            && (p is Connection && channel.ParentConnection.Name == ((Connection)p).Name)) == null)
            {
                MainPage.RunActionOnUiThread(() =>
                {
                    ObservableCollection<AHistory> col = ConnectionFile.Instance().Connections;

                    col.Insert(col.IndexOf(channel.ParentConnection) + 1, channel);
                    ((BIRCViewModel)MainPage.currentDataContext).ServerSelection = channel;
                });
            }
        }

        public static void RemoveChannel(Channel channel)
        {
            MainPage.RunActionOnUiThread(() =>
            {
                ObservableCollection<AHistory> col = ConnectionFile.Instance().Connections;

                ((BIRCViewModel)MainPage.currentDataContext).ServerSelection = channel.ParentConnection;
                col.Remove(channel);
            });
        }

        public static bool IsUserIgnored(Channel channel, string nickname)
        {
            if (channel.Users == null || channel.Users.Count == 0)
            {
                if (channel.Ignored == Channel.IGNORED)
                    return true;
                else
                    return false;
            }

            Channel user = channel.Users.FirstOrDefault(p => p.Name == nickname);
            if (user == null)
                return false;
            if (user.Ignored == Channel.IGNORED)
                return true;
            else
                return false;
        }

        public static AHistory GetActiveAHistory(Connection connection)
        {
            AHistory active = connection.Channels.FirstOrDefault(p => p.IsActive == true);

            if (active == null)
            {
                foreach (Channel cur in connection.Channels)
                {
                    active = cur.Users.FirstOrDefault(p => p.IsActive == true);
                    if (active != null)
                        break;
                }
                if (active == null)
                    active = connection;
            }
            return active;
        }

        public static string ColorFromStatus(IrcChannelUser user)
        {
            if (user.Modes.Contains('o'))
                return "Red";
            if (user.Modes.Contains('h'))
                return "Orange";
            if (user.Modes.Contains('v'))
                return "Green";
            return "Black";               
        }

        public static double OpacityFromStatus(IrcChannelUser user)
        {
            if (user.User.IsAway == true)
                return 0.4;
            return 1;
        }

        public static void UnreadChannelOnInactive(Channel curChannel)
        {
            if (!curChannel.IsActive)
            {
                MainPage.RunActionOnUiThread(() =>
                {
                    if (curChannel.Unread == 1)
                        curChannel.Unread = 2;
                    else
                        curChannel.Unread = 1;
                });
            }
        }
    }
}
