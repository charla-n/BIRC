using BIRC.ViewModels;
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
            this.Loaded += AddServerPage_Loaded;
            this.Unloaded += AddServerPage_Unloaded;
        }

        private void AddServerPage_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= AddServerPage_Loaded;
            this.Unloaded -= AddServerPage_Unloaded;
        }

        private void AddServerPage_Loaded(object sender, RoutedEventArgs e)
        {
            MainPage.currentDataContext = DataContext as ViewModelBase;
        }

        private void PasswordRequiredToggleBtn_Toggled(object sender, RoutedEventArgs e)
        {
            if (!PasswordRequiredToggleBtn.IsOn)
            {
                PasswordTxtBox.Visibility = Visibility.Collapsed;
                PasswordTxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                PasswordTxtBox.Visibility = Visibility.Visible;
                PasswordTxt.Visibility = Visibility.Visible;
            }
        }

        private void AddConnectionBtn_Click(object sender, RoutedEventArgs e)
        {
            ((AddServerViewModel)DataContext).SetPassword(PasswordTxtBox.Password);
        }
    }
}
