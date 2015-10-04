using BIRC.Shared.Models;
using BIRC.Shared.Utils;
using IrcDotNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Resources;

namespace BIRC.Shared.Commands
{
    public class Command
    {
        private Connection connection;
        private StandardIrcClient client;

        public Command()
        {
        }

        public Connection Connection
        {
            set
            {
                connection = value;
            }
        }

        public IrcLocalUser LocalUser
        {
            get
            {
                return client.LocalUser;
            }
        }

        public bool IsConnected()
        {
            if (client == null)
                return false;
            return client.IsConnected;
        }

        public void ListChannel(IEnumerable<string> list)
        {
            client.ListChannels(list);
        }

        public void WriteToHistory(string msg)
        {
            connection.AddHistory(HtmlWriter.Write(msg));
        }

        public void Info()
        {
            connection.AddHistory(HtmlWriter.Write(client.LocalUser.ServerInfo));
        }

        public void Disconnect()
        {
            client.Disconnect();
            client.Dispose();
            client = null;
        }

        public void Away(string msg = null)
        {
            if (msg != null)
                client.LocalUser.SetAway(msg);
            else
                client.LocalUser.UnsetAway();
        }

        public void SendMsg(string msg)
        {
            client.SendRawMessage(msg);
        }

        public void Join(IEnumerable<string> channels)
        {
            client.Channels.Join(channels);
        }

        public void Join(IEnumerable<Tuple<string, string>> channels)
        {
            client.Channels.Join(channels);
        }

        public async void Connect()
        {
            client = new StandardIrcClient();

            client.ChannelListReceived += Client_ChannelListReceived;
            client.Connected += Client_Connected;
            client.ConnectFailed += Client_ConnectFailed;
            client.Disconnected += Client_Disconnected;
            client.Error += Client_Error;
            client.ErrorMessageReceived += Client_ErrorMessageReceived;
            client.MotdReceived += Client_MotdReceived;
            client.ProtocolError += Client_ProtocolError;
            client.RawMessageReceived += Client_RawMessageReceived;
            client.Registered += Client_Registered;
            client.ServerBounce += Client_ServerBounce;
            client.ServerLinksListReceived += Client_ServerLinksListReceived;
            client.ServerStatsReceived += Client_ServerStatsReceived;
            client.ServerTimeReceived += Client_ServerTimeReceived;
            client.ServerVersionInfoReceived += Client_ServerVersionInfoReceived;
            client.WhoIsReplyReceived += Client_WhoIsReplyReceived;
            client.WhoReplyReceived += Client_WhoReplyReceived;
            client.WhoWasReplyReceived += Client_WhoWasReplyReceived;

            connection.AddHistory(HtmlWriter.Write(string.Format(MainPage.GetInfoString("TryToConnect"),
                connection.Server.Name, connection.Server.Port == null ? IrcClient.DefaultPort : (int)connection.Server.Port)));
            if (string.IsNullOrWhiteSpace(connection.Nickname))
                connection.Nickname = App.APPNAME + new Random().Next(int.MinValue, int.MaxValue);
            if (string.IsNullOrWhiteSpace(connection.RealName))
                connection.RealName = connection.Nickname;
            client.Connect(connection.Server.Name, connection.Server.Port == null ? IrcClient.DefaultPort : (int)connection.Server.Port,
                false, new IrcUserRegistrationInfo()
                {
                    NickName = connection.Nickname,
                    Password = string.IsNullOrWhiteSpace(connection.Password) ? null : await Encryption.UnProtect(connection.Password),
                    RealName = connection.RealName,
                    UserModes = connection.UserModes,
                    UserName = App.APPNAME
                });
        }

        private void Client_WhoWasReplyReceived(object sender, IrcUserEventArgs e)
        {
        }

        private void Client_WhoReplyReceived(object sender, IrcNameEventArgs e)
        {
        }

        private void Client_WhoIsReplyReceived(object sender, IrcUserEventArgs e)
        {
        }

        private void Client_ServerVersionInfoReceived(object sender, IrcServerVersionInfoEventArgs e)
        {
            connection.AddHistory(HtmlWriter.Write(string.Format(MainPage.GetInfoString("SrvVersion"),
                e.ServerName, e.Comments, e.Version, e.DebugLevel)));
        }

        private void Client_ServerTimeReceived(object sender, IrcServerTimeEventArgs e)
        {
            connection.AddHistory(HtmlWriter.Write(string.Format(MainPage.GetInfoString("SrvTime"),
                e.ServerName, e.DateTime)));
        }

        private void Client_ServerStatsReceived(object sender, IrcServerStatsReceivedEventArgs e)
        {
        }

        private void Client_ServerLinksListReceived(object sender, IrcServerLinksListReceivedEventArgs e)
        {
        }

        private void Client_ServerBounce(object sender, IrcServerInfoEventArgs e)
        {
        }

        private void Client_Registered(object sender, EventArgs e)
        {
            connection.AddHistory(HtmlWriter.Write(string.Format(MainPage.GetInfoString("Registered"), connection.Nickname)));
            client.LocalUser.InviteReceived += LocalUser_InviteReceived;
            client.LocalUser.JoinedChannel += LocalUser_JoinedChannel;
            client.LocalUser.LeftChannel += LocalUser_LeftChannel;
            client.LocalUser.MessageReceived += LocalUser_MessageReceived;
            client.LocalUser.MessageSent += LocalUser_MessageSent;
            client.LocalUser.ModesChanged += LocalUser_ModesChanged;
            client.LocalUser.NickNameChanged += LocalUser_NickNameChanged;
            client.LocalUser.NoticeReceived += LocalUser_NoticeReceived;
            client.LocalUser.NoticeSent += LocalUser_NoticeSent;
        }

        private void Client_RawMessageReceived(object sender, IrcRawMessageEventArgs e)
        {
            string res = ReceivedCommandParser.Parse(e.Message.Command, (string[])e.Message.Parameters, connection);

            if (res == ReceivedCommandParser.IGNORE)
                return;
            else if (res == null)
                connection.AddHistory(HtmlWriter.Write(e.RawContent));
            else
                connection.AddHistory(HtmlWriter.Write(res));
        }

        private void Client_ProtocolError(object sender, IrcProtocolErrorEventArgs e)
        {
            connection.AddHistory(HtmlWriter.WriteError(e.Message));
        }

        private void Client_MotdReceived(object sender, EventArgs e)
        {
        }

        private void Client_ErrorMessageReceived(object sender, IrcErrorMessageEventArgs e)
        {
            connection.AddHistory(HtmlWriter.WriteError(e.Message));
        }

        private void Client_Error(object sender, IrcErrorEventArgs e)
        {
            connection.AddHistory(HtmlWriter.WriteError(e.Error.Message));
        }

        private void Client_Disconnected(object sender, EventArgs e)
        {
            connection.Connected = false;
            connection.AddHistory(HtmlWriter.Write(string.Format(MainPage.GetInfoString("Disconnected"),
                connection.Server.Name, connection.Server.Port == null ? IrcClient.DefaultPort : (int)connection.Server.Port)));
            Unregister();
        }

        private void Client_ConnectFailed(object sender, IrcErrorEventArgs e)
        {
            connection.AddHistory(HtmlWriter.WriteError(e.Error.Message));
            Unregister();
        }

        private void Client_Connected(object sender, EventArgs e)
        {
            connection.Connected = true;
            connection.AddHistory(HtmlWriter.Write(string.Format(MainPage.GetInfoString("Connected"),
                connection.Server.Name, connection.Server.Port == null ? IrcClient.DefaultPort : (int)connection.Server.Port)));
        }

        private void Client_ChannelListReceived(object sender, IrcChannelListReceivedEventArgs e)
        {
            StringBuilder builder = new StringBuilder();

            foreach (IrcChannelInfo info in e.Channels)
                builder.Append(HtmlWriter.Write(string.Format(MainPage.GetInfoString("ListChannel"), info.Name, info.Topic, info.VisibleUsersCount)));
            if (e.Channels.Count == 0)
                builder.Append(HtmlWriter.Write(MainPage.GetInfoString("NoChannels")));
            connection.AddHistory(builder.ToString());
        }

        private void LocalUser_NoticeSent(object sender, IrcMessageEventArgs e)
        {
        }

        private void LocalUser_NoticeReceived(object sender, IrcMessageEventArgs e)
        {
        }

        private void LocalUser_NickNameChanged(object sender, EventArgs e)
        {
        }

        private void LocalUser_ModesChanged(object sender, EventArgs e)
        {
        }

        private void LocalUser_MessageSent(object sender, IrcMessageEventArgs e)
        {
        }

        private void LocalUser_MessageReceived(object sender, IrcMessageEventArgs e)
        {
        }

        private void LocalUser_LeftChannel(object sender, IrcChannelEventArgs e)
        {
        }

        private void LocalUser_JoinedChannel(object sender, IrcChannelEventArgs e)
        {
        }

        private void LocalUser_InviteReceived(object sender, IrcChannelInvitationEventArgs e)
        {
        }

        public void Unregister()
        {
            client.ChannelListReceived -= Client_ChannelListReceived;
            client.Connected -= Client_Connected;
            client.ConnectFailed -= Client_ConnectFailed;
            client.Disconnected -= Client_Disconnected;
            client.Error -= Client_Error;
            client.ErrorMessageReceived -= Client_ErrorMessageReceived;
            client.MotdReceived -= Client_MotdReceived;
            client.ProtocolError -= Client_ProtocolError;
            client.RawMessageReceived -= Client_RawMessageReceived;
            client.Registered -= Client_Registered;
            client.ServerBounce -= Client_ServerBounce;
            client.ServerLinksListReceived -= Client_ServerLinksListReceived;
            client.ServerStatsReceived -= Client_ServerStatsReceived;
            client.ServerTimeReceived -= Client_ServerTimeReceived;
            client.ServerVersionInfoReceived -= Client_ServerVersionInfoReceived;
            client.WhoIsReplyReceived -= Client_WhoIsReplyReceived;
            client.WhoReplyReceived -= Client_WhoReplyReceived;
            client.WhoWasReplyReceived -= Client_WhoWasReplyReceived;
            UnregisterLocalUser();
        }

        public void UnregisterLocalUser()
        {
            client.LocalUser.InviteReceived -= LocalUser_InviteReceived;
            client.LocalUser.JoinedChannel -= LocalUser_JoinedChannel;
            client.LocalUser.LeftChannel -= LocalUser_LeftChannel;
            client.LocalUser.MessageReceived -= LocalUser_MessageReceived;
            client.LocalUser.MessageSent -= LocalUser_MessageSent;
            client.LocalUser.ModesChanged -= LocalUser_ModesChanged;
            client.LocalUser.NickNameChanged -= LocalUser_NickNameChanged;
            client.LocalUser.NoticeReceived -= LocalUser_NoticeReceived;
            client.LocalUser.NoticeSent -= LocalUser_NoticeSent;
        }
    }
}
