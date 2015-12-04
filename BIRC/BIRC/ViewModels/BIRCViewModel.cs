﻿using BIRC.Shared.Commands;
using BIRC.Shared.Exceptions;
using BIRC.Shared.Files;
using BIRC.Shared.Models;
using BIRC.Shared.Utils;
using BIRC.Views;
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

namespace BIRC.ViewModels
{
    public class BIRCViewModel : ViewModelBase
    {
        public event Action OnAfterServerSelectionChanged;
        public event Action OnBeforeServerSelectionChanged;

        private Connection defaultConnection;
        private string commandTxt;
        private object serverSelection;
        private object userSelection;
        private RelayCommand AddConnectionCmd;
        private RelayCommand ConnectionCmd;
        private RelayCommand CommandCmd;

        public BIRCViewModel()
        {
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
        }

        public AHistory GetSelectedConnection()
        {
            if (serverSelection == null)
                return defaultConnection;
            if (serverSelection is AHistory)
                return (AHistory)serverSelection;
            return (AHistory)((TextBlock)serverSelection).DataContext;
        }

        public void CommandAction()
        {
            try
            {
                AHistory c = GetSelectedConnection();
                commandTxt = commandTxt.Trim();
                if (commandTxt.StartsWith("/"))
                {
                    string[] splitted = commandTxt.Split(PromptCommandParser.SEPARATORS);
                    PromptCommandParser.Parse(splitted[0], c, splitted.ToList());
                }
                else
                    PromptCommandParser.Parse(commandTxt, c, null);
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
            Connection co = GetSelectedConnection() as Connection;

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
                co = GetSelectedConnection() as Connection;
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

        public IEnumerable<AHistory> ByServers
        {
            get
            {
                return ConnectionFile.Instance().Connections;
            }
        }

        public IEnumerable<AHistory> UserList
        {
            get
            {
                Channel channel = GetSelectedConnection() as Channel;

                if (channel != null)
                    return channel.Users;
                return new List<AHistory>();
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
                MakeUserDisabled();
                GetSelectedConnection().IsActive = false;
                serverSelection = value;
                GetSelectedConnection().IsActive = true;
                OnAfterServerSelectionChanged?.Invoke();
                OnPropertyChanged("ServerSelection");
                OnPropertyChanged("UserList");
            }
        }

        public object UserSelection {
            get
            {
                return userSelection;
            }
            set
            {
                if (value != null)
                {
                    OnBeforeServerSelectionChanged?.Invoke();
                    GetSelectedConnection().IsActive = false;
                    userSelection = value;
                    serverSelection = value;
                    GetSelectedConnection().IsActive = true;
                    OnAfterServerSelectionChanged?.Invoke();
                    OnPropertyChanged("ServerSelection");
                }
            }
        }

        private void MakeUserDisabled()
        {
            AHistory unk = GetSelectedConnection();
            Channel channel;

            if (unk is Channel)
            {
                channel = unk as Channel;
                if (channel.Users != null)
                {
                    foreach (Channel user in channel.Users)
                        user.IsActive = false;
                }
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
