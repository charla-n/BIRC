using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using IrcDotNet;

namespace BIRC.Shared.Models
{
    public class Channel : AHistory
    {
        public static string IGNORED = "BlockContact";
        public static string NOT_IGNORED = "Contact";

        public Channel()
        {
            users = new ObservableCollection<Channel>();
            history = string.Empty;
            unread = 0;
            ignored = NOT_IGNORED;
            Color = "Black";
        }

        [JsonIgnore]
        private double opacity;
        [JsonIgnore]
        public double Opacity
        {
            get
            {
                return opacity;
            }
            set
            {
                opacity = value;
                MainPage.RunActionOnUiThread(() =>
                {
                    OnPropertyChanged("Opacity");
                });
            }
        }
        [JsonIgnore]
        private string color;
        [JsonIgnore]
        public string Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                MainPage.RunActionOnUiThread(() =>
                {
                    OnPropertyChanged("Color");
                });
            }
        }
        [JsonIgnore]
        private string ignored;
        [JsonIgnore]
        public string Ignored
        {
            get
            {
                return ignored;
            }
            set
            {
                ignored = value;
                MainPage.RunActionOnUiThread(() =>
                {
                    OnPropertyChanged("Ignored");
                });
            }
        }
        [JsonIgnore]
        private int unread;
        [JsonIgnore]
        public int Unread
        {
            get
            {
                return unread;
            }
            set
            {
                unread = value;
                MainPage.RunActionOnUiThread(() =>
                {
                    OnPropertyChanged("Unread");
                });
            }
        }
        [JsonIgnore]
        public IrcUser IrcUser { get; set; }
        [JsonIgnore]
        private ObservableCollection<Channel> users;
        [JsonIgnore]
        public ObservableCollection<Channel> Users {
            get
            {
                return users;
            }
            set
            {
                users = value;
                MainPage.RunActionOnUiThread(() =>
                {
                    MainPage.currentDataContext.Changed("UserList");
                });
            }
        }
        [JsonIgnore]
        public string RealName { get; set; }
        [JsonIgnore]
        public Connection ParentConnection { get; set; }

        public void AddUser(Channel user)
        {
            MainPage.RunActionOnUiThread(() =>
            {
                Users.Add(user);
            });
        }

        public void RemoveUser(Channel user)
        {
            MainPage.RunActionOnUiThread(() =>
            {
                Users.Remove(user);
            });
        }
    }
}
