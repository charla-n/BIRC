using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BIRC.Views
{
    public sealed partial class ServerListUserControl : UserControl
    {
        public ServerListUserControl()
        {
            this.InitializeComponent();
        }

        private void PortTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PortTxtBox.Text.Length == 0)
                return;
            try
            {
                int.Parse(PortTxtBox.Text);
            }
            catch (Exception)
            {
                PortTxtBox.Text = PortTxtBox.Text.Remove(PortTxtBox.Text.Length - 1);
                PortTxtBox.SelectionStart = PortTxtBox.Text.Length;
            }
        }

        private void ListViewServer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewServer.ScrollIntoView(ListViewServer.SelectedItem);
        }
    }
}
