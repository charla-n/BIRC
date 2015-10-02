using BIRC.Shared.Models;
using BIRC.Shared.Utils;
using IrcDotNet;
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
            client = new StandardIrcClient();
        }

        public Connection Connection
        {
            set
            {
                connection = value;
            }
        }

        public void Unregister()
        {
            client.ChannelListReceived -= Client_ChannelListReceived;
            client.ClientInfoReceived -= Client_ClientInfoReceived;
            client.Connected -= Client_Connected;
            client.ConnectFailed -= Client_ConnectFailed;
            client.Disconnected -= Client_Disconnected;
            client.Error -= Client_Error;
            client.ErrorMessageReceived -= Client_ErrorMessageReceived;
            client.MotdReceived -= Client_MotdReceived;
            client.NetworkInformationReceived -= Client_NetworkInformationReceived;
            client.ProtocolError -= Client_ProtocolError;
            client.RawMessageReceived -= Client_RawMessageReceived;
            client.Registered -= Client_Registered;
            client.ServerBounce -= Client_ServerBounce;
            client.ServerLinksListReceived -= Client_ServerLinksListReceived;
            client.ServerStatsReceived -= Client_ServerStatsReceived;
            client.ServerSupportedFeaturesReceived -= Client_ServerSupportedFeaturesReceived;
            client.ServerTimeReceived -= Client_ServerTimeReceived;
            client.ServerVersionInfoReceived -= Client_ServerVersionInfoReceived;
            client.WhoIsReplyReceived -= Client_WhoIsReplyReceived;
            client.WhoReplyReceived -= Client_WhoReplyReceived;
            client.WhoWasReplyReceived -= Client_WhoWasReplyReceived;
        }

        public async void Connect()
        {
            client.ChannelListReceived += Client_ChannelListReceived;
            client.ClientInfoReceived += Client_ClientInfoReceived;
            client.Connected += Client_Connected;
            client.ConnectFailed += Client_ConnectFailed;
            client.Disconnected += Client_Disconnected;
            client.Error += Client_Error;
            client.ErrorMessageReceived += Client_ErrorMessageReceived;
            client.MotdReceived += Client_MotdReceived;
            client.NetworkInformationReceived += Client_NetworkInformationReceived;
            client.ProtocolError += Client_ProtocolError;
            client.RawMessageReceived += Client_RawMessageReceived;
            client.Registered += Client_Registered;
            client.ServerBounce += Client_ServerBounce;
            client.ServerLinksListReceived += Client_ServerLinksListReceived;
            client.ServerStatsReceived += Client_ServerStatsReceived;
            client.ServerSupportedFeaturesReceived += Client_ServerSupportedFeaturesReceived;
            client.ServerTimeReceived += Client_ServerTimeReceived;
            client.ServerVersionInfoReceived += Client_ServerVersionInfoReceived;
            client.WhoIsReplyReceived += Client_WhoIsReplyReceived;
            client.WhoReplyReceived += Client_WhoReplyReceived;
            client.WhoWasReplyReceived += Client_WhoWasReplyReceived;

            connection.History += HtmlWriter.Write(string.Format(MainPage.GetString("TryToConnect"),
                connection.Server.Name, connection.Server.Port == null ? IrcClient.DefaultPort : (int)connection.Server.Port));
            client.Connect(connection.Server.Name, connection.Server.Port == null ? IrcClient.DefaultPort : (int)connection.Server.Port,
                false, new IrcUserRegistrationInfo()
                {
                    NickName = string.IsNullOrWhiteSpace(connection.Nickname) ? connection.Username : connection.Nickname,
                    Password = string.IsNullOrWhiteSpace(connection.Password) ? null : await Encryption.UnProtect(connection.Password),
                    RealName = connection.RealName == null ? "" : connection.RealName,
                    UserModes = connection.UserModes,
                    UserName = connection.Username
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
        }

        private void Client_ServerTimeReceived(object sender, IrcServerTimeEventArgs e)
        {
        }

        private void Client_ServerSupportedFeaturesReceived(object sender, EventArgs e)
        {
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
            connection.History += HtmlWriter.Write(string.Format(MainPage.GetString("Registered"), connection.Nickname));
        }

        private void Client_RawMessageReceived(object sender, IrcRawMessageEventArgs e)
        {
            connection.History += HtmlWriter.Write(e.RawContent);
        }

        private void Client_ProtocolError(object sender, IrcProtocolErrorEventArgs e)
        {
        }

        private void Client_NetworkInformationReceived(object sender, IrcCommentEventArgs e)
        {
        }

        private void Client_MotdReceived(object sender, EventArgs e)
        {
        }

        private void Client_ErrorMessageReceived(object sender, IrcErrorMessageEventArgs e)
        {
        }

        private void Client_Error(object sender, IrcErrorEventArgs e)
        {
        }

        private void Client_Disconnected(object sender, EventArgs e)
        {
            connection.History += HtmlWriter.Write(string.Format(MainPage.GetString("Disconnected"),
                connection.Server.Name, connection.Server.Port == null ? IrcClient.DefaultPort : (int)connection.Server.Port));
            Unregister();
        }

        private void Client_ConnectFailed(object sender, IrcErrorEventArgs e)
        {
            Unregister();
        }

        private void Client_Connected(object sender, EventArgs e)
        {
            connection.History += HtmlWriter.Write(string.Format(MainPage.GetString("Connected"),
                connection.Server.Name, connection.Server.Port == null ? IrcClient.DefaultPort : (int)connection.Server.Port));
        }

        private void Client_ClientInfoReceived(object sender, EventArgs e)
        {
        }

        private void Client_ChannelListReceived(object sender, IrcChannelListReceivedEventArgs e)
        {
        }
    }
}
