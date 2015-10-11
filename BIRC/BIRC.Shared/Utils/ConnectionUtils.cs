using BIRC.Shared.Files;
using BIRC.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BIRC.Shared.Utils
{
    public static class ConnectionUtils
    {
        public static Connection ConnectionFromAHistory(AHistory obj)
        {
            Connection co = null;

            if (obj is Connection)
                co = obj as Connection;
            else
                co = ((Channel)obj).ParentConnection;

            return co;
        }

        public static Connection Add(Connection c)
        {
            Connection get = ConnectionFile.Instance().Connections.FirstOrDefault(
                p => p.Server.Name == c.Server.Name);
            if (get != null)
                return get;
            ConnectionFile.Instance().Connections.Add(c);
            MainPage.currentDataContext.Changed("ByServers");
            ConnectionFile.Instance().WriteImpl(ConnectionFile.Instance().Connections);
            return c;
        }
    }
}
