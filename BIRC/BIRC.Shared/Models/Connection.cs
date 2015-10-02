using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIRC.Shared.Models
{
    public class Connection
    {
        [JsonIgnore]
        public const string DEFAULT_GROUP = "Default";

        public Server Server { get; set; }
        public string Username { get; set; }
        public bool RequirePassword { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public string RealName { get; set; }
        public string Group { get; set; }
        public char[] UserModes { get; set; }
        public bool AutoConnect { get; set; }
    }
}
