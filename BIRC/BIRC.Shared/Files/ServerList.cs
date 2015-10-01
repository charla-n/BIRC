using BIRC.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BIRC.Shared.Files
{
    public class ServerList : AbstractFiles<List<Server>>
    {
        private static ServerList INSTANCE = new ServerList();
        private const string FILE = "serverlist.json";

        private ServerList() : base(FILE)
        { }

        public static ServerList Instance()
        {
            return INSTANCE;
        }

        public override Task<List<Server>> ReadImpl()
        {
            return Read();
        }

        public override void WriteImpl(List<Server> obj)
        {
            Write(obj);
        }
    }
}
