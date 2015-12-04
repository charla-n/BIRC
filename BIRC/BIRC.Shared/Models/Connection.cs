using BIRC.Shared.Commands;
using BIRC.Shared.Utils;
using BIRC.ViewModels;
using IrcDotNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Windows.UI.Core;

namespace BIRC.Shared.Models
{
    public abstract class AHistory : INotifyPropertyChanged
    {
        public event Action<string> OnAddHistory;

        public AHistory()
        {
            IsActive = false;
            CommandHistory = new HistoryList();
        }

        public string name { get; set; }
        [JsonIgnore]
        public string Name {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        [JsonIgnore]
        public Command Command { get; set; }
        [JsonIgnore]
        protected string history;
        [JsonIgnore]
        public HistoryList CommandHistory { get; set; }
        [JsonIgnore]
        public string History
        {
            get
            {
                return history;
            }
        }

        [JsonIgnore]
        public bool IsActive { get; set; }

        public void AddHistory(string historyToAdd)
        {
            history += historyToAdd;
            if (IsActive)
                OnAddHistory?.Invoke(historyToAdd);
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

    public class Connection : AHistory
    {
        public Connection()
        {
            Command = new Command();
            Channels = new List<Channel>();
            Command.Connection = this;
            history = string.Empty;
        }

        public int? Port { get; set; }
        public bool RequirePassword { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public string RealName { get; set; }
        public char[] UserModes { get; set; }
        public bool AutoConnect { get; set; }

        [JsonIgnore]
        public List<Channel> Channels { get; set; }
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
                {
                    MainPage.currentDataContext.Changed("ConnectTxt");
                });
            }
        }
        [JsonIgnore]
        public bool IsDefault { get; set; }

        public void AddChannel(Channel channel)
        {
            Channels.Add(channel);
        }
    }
}
