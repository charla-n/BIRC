using BIRC.Shared.Files;
using BIRC.Shared.Models;
using BIRC.Shared.Utils;
using CollectionView;
using FTR.Shared.Utils;
using IrcDotNet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Animation;

namespace BIRC.ViewModels
{
    public class AddServerViewModel : ViewModelBase
    {
        private ListCollectionView list;
        private string txtSearch;
        private string txtHostname;
        private string txtPort;
        private string txtComment;
        private bool passwordRequired;
        private string nickname;
        private string realname;
        private bool autoConnect;
        private string password;
        private bool isInvisible;
        private bool isWallops;
        private Server listSelectedItem;
        private RelayCommand saveAsNewCmd;
        private RelayCommand overwriteCmd;
        private RelayCommand removeCmd;
        private RelayCommand addConnection;

        public AddServerViewModel()
        {
            saveAsNewCmd = new RelayCommand(SaveAsNewAction, CanSaveAsNew);
            overwriteCmd = new RelayCommand(OverwriteAction, CanOverwrite);
            removeCmd = new RelayCommand(RemoveAction, CanRemove);
            addConnection = new RelayCommand(AddConnectionAction, CanAddConnection);
            txtSearch = string.Empty;
            list = new ListCollectionView();
            list.Filter = p =>
            {
                if (!((Server)p).Name.Contains(txtSearch))
                    return false;
                return true;
            };
            RetrieveList();
        }

        public void SetPassword(string password)
        {
            this.password = password;
        }

        private async void RetrieveList()
        {
            await ServerList.Instance().ReadImpl().ContinueWith(p =>
            {
                List<Server> res = p.Result;
                MainPage.RunActionOnUiThread(() =>
                {
                    list.Source = res;
                    OnPropertyChanged("List");
                });
            });
        }

        private void EmptyServerControls()
        {
            txtHostname = null;
            txtPort = null;
            txtComment = null;
        }

        private async void AddConnectionAction()
        {
            List<char> modes = new List<char>();

            if (isInvisible)
                modes.Add('i');
            if (isWallops)
                modes.Add('w');
            ConnectionUtils.Add(new Connection()
            {
                AutoConnect = autoConnect,
                Nickname = nickname,
                Password = await Encryption.Protect(password),
                RealName = realname,
                RequirePassword = passwordRequired,
                Name = txtHostname,
                Port = txtPort == null ? null : (int?)int.Parse(txtPort),
                UserModes = modes.ToArray()
            });
            password = null;
            NavigateToMainPage();
        }

        private void NavigateToMainPage()
        {
            var frame = (Frame)Window.Current.Content;

            frame.Navigate(typeof(MainPage));
        }

        private bool CanAddConnection()
        {
            if (ListSelectedItem == null)
                return false;
            return true;
        }

        private void SaveAsNewAction()
        {
            Server srv = new Server()
            {
                Comment = txtComment,
                Name = txtHostname,
                Port = txtPort == null ? null : (int?)int.Parse(txtPort)
            };
            list.Add(srv);
            list.Refresh();
            ListSelectedItem = srv;
            OnPropertyChanged("ListSelectedItem");
            ServerList.Instance().WriteImpl((List<Server>)list.Source);
        }

        private bool CanSaveAsNew()
        {
            if (string.IsNullOrWhiteSpace(txtHostname))
                return false;
            return true;
        }

        private void OverwriteAction()
        {
            list.EditItem(ListSelectedItem);
            listSelectedItem.Comment = txtComment;
            listSelectedItem.Name = txtHostname;
            listSelectedItem.Port = txtPort == null ? null : (int?)int.Parse(txtPort);
            list.CommitEdit();
            list.Refresh();
            OnPropertyChanged("ListSelectedItem");
            ServerList.Instance().WriteImpl((List<Server>)list.Source);
        }

        private bool CanOverwrite()
        {
            if (ListSelectedItem == null)
                return false;
            if (string.IsNullOrWhiteSpace(txtHostname))
                return false;
            return true;
        }

        private void RemoveAction()
        {
            list.Remove(ListSelectedItem);
            list.Refresh();
            ListSelectedItem = null;
            OnPropertyChanged("ListSelectedItem");
            ServerList.Instance().WriteImpl((List<Server>)list.Source);
        }

        private bool CanRemove()
        {
            if (ListSelectedItem == null)
                return false;
            return true;
        }

        #region COMMAND

        public ICommand AddConnection
        {
            get
            {
                return addConnection;
            }
        }

        public ICommand Remove {
            get
            {
                return removeCmd;
            }
        }

        public ICommand Overwrite {
            get
            {
                return overwriteCmd;
            }
        }

        public ICommand SaveAsNew {
            get
            {
                return saveAsNewCmd;
            }
        }

        #endregion

        public Server ListSelectedItem {
            get
            {
                return listSelectedItem;
            }
            set
            {
                listSelectedItem = value;
                if (listSelectedItem != null)
                {
                    txtHostname = listSelectedItem.Name;
                    txtPort = listSelectedItem.Port == null ? IrcClient.DefaultPort.ToString() : listSelectedItem.Port.ToString();
                    txtComment = listSelectedItem.Comment;
                    OnPropertyChanged("TxtHostname");
                    OnPropertyChanged("TxtPort");
                    OnPropertyChanged("TxtComment");
                }
                overwriteCmd.RaiseCanExecuteChanged();
                saveAsNewCmd.RaiseCanExecuteChanged();
                removeCmd.RaiseCanExecuteChanged();
                addConnection.RaiseCanExecuteChanged();
            }
        }

        public string TxtHostname
        {
            get
            {
                return txtHostname;
            }
            set
            {
                txtHostname = value;
                overwriteCmd.RaiseCanExecuteChanged();
                saveAsNewCmd.RaiseCanExecuteChanged();
            }
        }

        public bool IsInvisible {
            get
            {
                return isInvisible;
            }
            set
            {
                isInvisible = value;
            }
        }

        public bool IsWallops
        {
            get
            {
                return isWallops;
            }
            set
            {
                isWallops = value;
            }
        }

        public string TxtPort
        {
            get
            {
                return txtPort;
            }
            set
            {
                txtPort = value;
            }
        }


        public string TxtComment
        {
            get
            {
                return txtComment;
            }
            set
            {
                txtComment = value;
            }
        }

        public ICollectionView List
        {
            get
            {
                return list;
            }
        }

        public string TxtSearch
        {
            get
            {
                return txtSearch;
            }
            set
            {
                txtSearch = value;
                OnPropertyChanged("TxtSearch");
                list.Refresh();
            }
        }

        public bool PasswordRequired
        {
            get
            {
                return passwordRequired;
            }
            set
            {
                passwordRequired = value;
            }
        }

        public bool AutoConnect
        {
            get
            {
                return autoConnect;
            }
            set
            {
                autoConnect = value;
            }
        }

        public string Nickname
        {
            get
            {
                return nickname;
            }
            set
            {
                nickname = value;
            }
        }

        public string Realname
        {
            get
            {
                return realname;
            }
            set
            {
                realname = value;
            }
        }


        public override void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
