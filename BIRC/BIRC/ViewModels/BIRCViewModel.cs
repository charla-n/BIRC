using BIRC.Shared.Commands;
using BIRC.Shared.Exceptions;
using BIRC.Shared.Files;
using BIRC.Shared.Models;
using BIRC.Shared.Utils;
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
        public event Action OnAfterServerSelectionChanged;
        public event Action OnBeforeServerSelectionChanged;

        private Connection defaultConnection;
        private string commandTxt;
        private object serverSelection;
        private AHistory channelSelection;
        private RelayCommand AddConnectionCmd;
        private RelayCommand ConnectionCmd;
        private RelayCommand CommandCmd;

        public BIRCViewModel()
        {
            channelSelection = null;
            commandTxt = string.Empty;
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

        public AHistory GetSelectedConnection()
        {
            if (serverSelection == null)
                return defaultConnection;
            if (serverSelection is Connection)
                return (Connection)serverSelection;
            return (Connection)((TextBlock)serverSelection).DataContext;
        }

        public void CommandAction()
        {
            try
            {
                AHistory c = GetSelectedConnection();
                string[] splitted = commandTxt.Split(PromptCommandParser.SEPARATORS);
                PromptCommandParser.Parse(splitted[0], c, splitted.ToList());
            }
            catch (ErrorBIRC e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            GetSelectedConnection().CommandHistory.Add(commandTxt);
            CommandTxt = null;
        }

        private void ConnectionAction()
        {
            Connection co = ConnectionUtils.ConnectionFromAHistory(GetSelectedConnection());

            if (co.Command.IsConnected())
                co.Command.Disconnect();
            else
                co.Command.Connect();
        }

        public string ConnectTxt
        {
            get
            {
                Connection co = null;

                if (GetSelectedConnection() == null)
                    return "Connect";
                co = ConnectionUtils.ConnectionFromAHistory(GetSelectedConnection());
                if (co.Connected)
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

        public AHistory ChannelSelection
        {
            get
            {
                return channelSelection;
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
                OnBeforeServerSelectionChanged?.Invoke();
                serverSelection = value;
                OnAfterServerSelectionChanged?.Invoke();
                OnPropertyChanged("ServerSelection");
            }
        }

        private void AddConnectionAction()
        {
            var frame = (Frame)Window.Current.Content;

            frame.Navigate(typeof(AddServerPage), GetSelectedConnection());
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
