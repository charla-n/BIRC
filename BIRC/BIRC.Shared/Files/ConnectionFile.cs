using BIRC.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BIRC.Shared.Files
{
    public class ConnectionFile : AbstractFiles<List<Connection>>
    {
        private static ConnectionFile INSTANCE = new ConnectionFile();
        private const string FILE = "connections.json";

        private ConnectionFile() : base(FILE) { }

        public static ConnectionFile Instance()
        {
            return INSTANCE;
        }

        public override Task<List<Connection>> ReadImpl()
        {
            return Read();
        }

        public override void WriteImpl(List<Connection> obj)
        {
            Write(obj);
        }
    }
}
