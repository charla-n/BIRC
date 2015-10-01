using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BIRC
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static CoreDispatcher dispatcher;

        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
        }

        public async static void RunActionOnUiThread(DispatchedHandler action)
        {
            if (dispatcher != null)
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, action);
            else
                action.Invoke();
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            LeftPane.IsPaneOpen = !LeftPane.IsPaneOpen;
        }
    }
}
