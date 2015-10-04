using BIRC.Shared.Commands;
using BIRC.Shared.Utils;
using IrcDotNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Windows.UI.Core;

namespace BIRC.Shared.Models
{
//    <script>
//    window.setInterval(function()
//    {
//        var x = document.getElementsByClassName('bottom');
//        x[x.length - 1].scrollIntoView();
//    }, 100);
//</script>

    public class Connection : INotifyPropertyChanged
    {
        public event Action<string> OnAddHistory;

        public Connection()
        {
            Command = new Command();
            CommandHistory = new HistoryList();
            Command.Connection = this;
            Group = DEFAULT_GROUP;
            history = string.Empty;
        }

        [JsonIgnore]
        public const string DEFAULT_GROUP = "Default";

        public Server Server { get; set; }
        public bool RequirePassword { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public string RealName { get; set; }
        public string Group { get; set; }
        public char[] UserModes { get; set; }
        public bool AutoConnect { get; set; }

        [JsonIgnore]
        public List<Channel> Channels { get; set; }
        [JsonIgnore]
        public HistoryList CommandHistory { get; set; }
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
                {
                    MainPage.currentDataContext.Changed("ConnectTxt");
                });
            }
        }
        [JsonIgnore]
        public bool IsDefault { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void AddHistory(string historyToAdd)
        {
            history += historyToAdd;
            OnAddHistory?.Invoke(historyToAdd);
        }
    }
}
