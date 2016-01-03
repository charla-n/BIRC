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
        public Channel()
        {
            users = new ObservableCollection<Channel>();
            history = string.Empty;
            unread = 0;
            ignored = false;
        }

        [JsonIgnore]
        public bool ignored;
        [JsonIgnore]
        public bool Ignored
        {
            get
            {
                return ignored;
            }
            set
            {
                ignored = value;
                OnPropertyChanged("Ignored");
            }
        }
        [JsonIgnore]
        public int unread;
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
                OnPropertyChanged("Unread");
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
