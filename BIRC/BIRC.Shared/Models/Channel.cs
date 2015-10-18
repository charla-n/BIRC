using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BIRC.Shared.Models
{
    public class Channel : AHistory
    {
        public Channel()
        {
            users = new ObservableCollection<string>();
            history = string.Empty;
        }

        [JsonIgnore]
        private ObservableCollection<string> users;
        [JsonIgnore]
        public ObservableCollection<string> Users {
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

        public void AddUser(string user)
        {
            MainPage.RunActionOnUiThread(() =>
            {
                Users.Add(user);
            });
        }

        public void RemoveUser(string user)
        {
            MainPage.RunActionOnUiThread(() =>
            {
                Users.Remove(user);
            });
        }
    }
}
