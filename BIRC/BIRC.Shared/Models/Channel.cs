using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIRC.Shared.Models
{
    public class Channel : AHistory
    {
        public Channel()
        {
            history = string.Empty;
        }

        public string Name { get; set; }
        [JsonIgnore]
        public Connection ParentConnection { get; set; }
    }
}
