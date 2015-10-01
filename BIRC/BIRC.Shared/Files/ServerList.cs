using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BIRC.Shared.Files
{
    public class ServerList : AbstractFiles<List<string>>
    {
        private static ServerList INSTANCE = new ServerList();
        private static string FILE = "serverlist.json";

        private ServerList() : base(FILE) {}

        public static ServerList Instance()
        {
            return INSTANCE;
        }

        public override Task<List<string>> ReadImpl()
        {
            return Read();
        }

        public override void WriteImpl(List<string> obj)
        {
            Write(obj);
        }
    }
}
