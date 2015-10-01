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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BIRC.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddServerPage : Page
    {
        public AddServerPage()
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
    }
}
