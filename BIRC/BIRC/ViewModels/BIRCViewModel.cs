using BIRC.Shared.Files;
using BIRC.Shared.Models;
using BIRC.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BIRC.ViewModels
{
    public class BIRCViewModel : ViewModelBase
    {
        private object serverSelection;
        private RelayCommand AddConnectionCmd;
        private RelayCommand ConnectionCmd;

        public BIRCViewModel()
        {
            RetrieveList();
            AddConnectionCmd = new RelayCommand(AddConnectionAction, () => true);
            ConnectionCmd = new RelayCommand(ConnectionAction, () => true);
        }

        public async void RetrieveList()
        {
            await ConnectionFile.Instance().ReadImpl();
            OnPropertyChanged("ByServers");
        }

        public Connection GetSelectedConnection()
        {
            if (serverSelection == null)
                return null;
            if (serverSelection is Connection)
                return (Connection)serverSelection;
            return (Connection)((RelativePanel)serverSelection).DataContext;
        }

        private void ConnectionAction()
        {
            GetSelectedConnection().Command.Connect();
        }

        public string WebViewContent
        {
            get
            {
                if (GetSelectedConnection() == null)
                    return string.Empty;
                return GetSelectedConnection().History;
            }
        }

        public IEnumerable<object> ByServers
        {
            get
            {
                return ConnectionFile.Instance().Connections.GroupBy(x => x.Group)
                    .Select(x => new
                    {
                        Group = x.Key,
                        Items = x.ToList()
                    });
            }
        }

        public ICommand Connect
        {
            get
            {
                return ConnectionCmd;
            }
        }

        public ICommand AddConnection {
            get
            {
                return AddConnectionCmd;
            }
        }

        public object ServerSelection {
            get
            {
                return serverSelection;
            }
            set
            {
                if (GetSelectedConnection() != null)
                    GetSelectedConnection().PropertyChanged -= BIRCViewModel_PropertyChanged;
                serverSelection = value;
                if (GetSelectedConnection() != null)
                    GetSelectedConnection().PropertyChanged += BIRCViewModel_PropertyChanged;
            }
        }

        private void BIRCViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Connection.History))
            {
                OnPropertyChanged("WebViewContent");
            }
        }

        private void AddConnectionAction()
        {
            var frame = (Frame)Window.Current.Content;

            frame.Navigate(typeof(AddServerPage));
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
