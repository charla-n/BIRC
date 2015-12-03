using BIRC.Shared.Files;
using BIRC.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;
using BIRC.ViewModels;

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
    }
}
