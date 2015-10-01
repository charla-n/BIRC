using BIRC.Shared.Files;
using BIRC.Shared.Models;
using CollectionView;
using IrcDotNet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
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
        private Server listSelectedItem;

        public AddServerViewModel()
        {
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
            }
        }

        public string TxtHostname {
            get
            {
                return txtHostname;
            }
            set
            {
                txtHostname = value;
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

        public ICollectionView List {
            get
            {
                return list;
            }
        }

        public string TxtSearch {
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

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
