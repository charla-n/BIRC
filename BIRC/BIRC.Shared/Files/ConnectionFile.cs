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
        private List<Connection> connections;

        private ConnectionFile() : base(FILE)
        {
            connections = new List<Connection>();
        }

        public static ConnectionFile Instance()
        {
            return INSTANCE;
        }

        public List<Connection> Connections {
            get
            {
                return connections;
            }
            set
            {
                connections = value;
            }
        }

        public override async Task<List<Connection>> ReadImpl()
        {
            connections = await Read();
            if (connections == null)
                connections = new List<Connection>();
            return connections;
        }

        public override void WriteImpl(List<Connection> obj)
        {
            Write(obj);
        }
    }
}
