using BIRC.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace BIRC.Shared.Files
{
    public class ConnectionFile : AbstractFiles<List<Connection>>
    {
        private static ConnectionFile INSTANCE = new ConnectionFile();
        private const string FILE = "connections.json";
        private ObservableCollection<AHistory> connections;

        private ConnectionFile() : base(FILE)
        {
            connections = new ObservableCollection<AHistory>();
        }

        public static ConnectionFile Instance()
        {
            return INSTANCE;
        }

        public ObservableCollection<AHistory> Connections {
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
            List<Connection> co;
            
            co = await Read();
            if (co == null)
                connections = new ObservableCollection<AHistory>();
            else
                connections = new ObservableCollection<AHistory>(co);
            return co;
        }

        public override void WriteImpl(List<Connection> obj)
        {
            Write(obj);
        }
    }
}
