using BIRC.Shared.Commands;
using IrcDotNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Windows.UI.Core;

namespace BIRC.Shared.Models
{
    public class Connection : INotifyPropertyChanged
    {
        public Connection()
        {
            Command = new Command();
            Command.Connection = this;
        }

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

        [JsonIgnore]
        public Command Command { get; set; }
        [JsonIgnore]
        private string history;
        [JsonIgnore]
        public string History {
            get
            {
                return history;
            }
            set
            {
                history = value;
                MainPage.RunActionOnUiThread(() =>
                { OnPropertyChanged("History"); });
            }
        }
        [JsonIgnore]
        private bool connected;
        [JsonIgnore]
        public bool Connected {
            get
            {
                return connected;
            }
            set
            {
                connected = value;
                MainPage.RunActionOnUiThread(() =>
                { OnPropertyChanged("Connected"); });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
