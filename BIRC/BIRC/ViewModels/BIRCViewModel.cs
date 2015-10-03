using BIRC.Shared.Commands;
using BIRC.Shared.Exceptions;
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
        private Connection defaultConnection;
        private string commandTxt;
        private object serverSelection;
        private RelayCommand AddConnectionCmd;
        private RelayCommand ConnectionCmd;
        private RelayCommand CommandCmd;

        public BIRCViewModel()
        {
            defaultConnection = new Connection() { IsDefault = true };
            RetrieveList();
            AddConnectionCmd = new RelayCommand(AddConnectionAction, () => true);
            ConnectionCmd = new RelayCommand(ConnectionAction, () => true);
            CommandCmd = new RelayCommand(CommandAction, () => true);
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

        public void CommandAction()
        {
            try
            {
                Connection c = GetSelectedConnection();
                string[] splitted = commandTxt.Split(PromptCommandParser.SEPARATORS);
                PromptCommandParser.Parse(splitted[0], c == null ? defaultConnection : c, splitted.ToList());
            }
            catch (ErrorBIRC e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            CommandTxt = null;
        }

        private void ConnectionAction()
        {
            if (GetSelectedConnection().Command.IsConnected())
                GetSelectedConnection().Command.Disconnect();
            else
                GetSelectedConnection().Command.Connect();
        }

        public string WebViewContent
        {
            get
            {
                if (GetSelectedConnection() == null)
                    return defaultConnection.History;
                return GetSelectedConnection().History;
            }
        }

        public string ConnectTxt
        {
            get
            {
                if (GetSelectedConnection() == null)
                    return "Connect";
                if (GetSelectedConnection().Connected)
                    return "Disconnect";
                else
                    return "Connect";
            }
        }

        public string CommandTxt
        {
            get
            {
                return commandTxt;
            }
            set
            {
                commandTxt = value;
                OnPropertyChanged("CommandTxt");
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

        public ICommand CommandBtn
        {
            get
            {
                return CommandCmd;
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
                serverSelection = value;
                OnPropertyChanged("ServerSelection");
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
