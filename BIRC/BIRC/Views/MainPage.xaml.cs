using BIRC.Shared.Models;
using BIRC.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
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
        public static ViewModelBase currentDataContext;
        private static CoreDispatcher dispatcher;
        private static ResourceLoader infoloader;
        private static ResourceLoader errorloader;
        private ManualResetEvent resetEvent;
        private ManualResetEvent resetEventAddHistory;
        private WebView WebView;

        private string part;
        private string scrollToEnd;

        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
            part = "";
            scrollToEnd = "true";
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ((BIRCViewModel)currentDataContext).GetSelectedConnection().OnAddHistory -= MainPage_OnAddHistory;
            ((BIRCViewModel)currentDataContext).OnBeforeServerSelectionChanged -= CurrentDataContext_OnBeforeServerSelectionChanged;
            ((BIRCViewModel)currentDataContext).OnAfterServerSelectionChanged -= CurrentDataContext_OnAfterServerSelectionChanged;
            Loaded -= MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            infoloader = ResourceLoader.GetForCurrentView(App.RESOURCE_NAME);
            errorloader = ResourceLoader.GetForCurrentView(App.ERROR_RESOURCE_NAME);
            currentDataContext = DataContext as ViewModelBase;
            ((BIRCViewModel)currentDataContext).GetSelectedConnection().OnAddHistory += MainPage_OnAddHistory;
            ((BIRCViewModel)currentDataContext).OnBeforeServerSelectionChanged += CurrentDataContext_OnBeforeServerSelectionChanged;
            ((BIRCViewModel)currentDataContext).OnAfterServerSelectionChanged += CurrentDataContext_OnAfterServerSelectionChanged;
            resetEvent = new ManualResetEvent(false);
            resetEventAddHistory = new ManualResetEvent(false);
            WebView = new WebView();
            WebView.Navigate(new Uri("ms-appx-web:///assets/base.html"));
            WebViewContainer.Children.Add(WebView);
        }

        private void CurrentDataContext_OnAfterServerSelectionChanged()
        {
            Task.Run(async () =>
            {
                resetEvent.WaitOne();
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    ((BIRCViewModel)currentDataContext).GetSelectedConnection().OnAddHistory += MainPage_OnAddHistory;
                    await WebView.InvokeScriptAsync("replaceContent",
                        new string[] { ((BIRCViewModel)currentDataContext).GetSelectedConnection().History });
                    resetEventAddHistory.Set();
                });
                resetEvent.Reset();
            });
        }

        private async void CurrentDataContext_OnBeforeServerSelectionChanged()
        {
            resetEventAddHistory.Reset();
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ((BIRCViewModel)currentDataContext).GetSelectedConnection().OnAddHistory -= MainPage_OnAddHistory;
            });
            WebView.ScriptNotify -= WebView_ScriptNotify;
            WebView.LoadCompleted -= WebView_LoadCompleted;
            WebViewContainer.Children.Clear();
            GC.Collect();
            AddWebView();
        }

        private void AddWebView()
        {
            WebView = new WebView();
            WebView.LoadCompleted += WebView_LoadCompleted;
            WebView.Navigate(new Uri("ms-appx-web:///assets/base.html"));
            WebViewContainer.Children.Add(WebView);
            WebView.ScriptNotify += WebView_ScriptNotify;
        }

        private void WebView_ScriptNotify(object sender, NotifyEventArgs e)
        {
            if (e.Value == "stopScrollingToEnd")
            {
                scrollToEnd = "false";
            }
            else if (e.Value == "startScrollingToEnd")
            {
                scrollToEnd = "true";
            }
        }

        private void WebView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            resetEvent.Set();
        }

        private void MainPage_OnAddHistory(string obj)
        {
            Task.Run(async () =>
            {
                resetEventAddHistory.WaitOne();
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    await WebView.InvokeScriptAsync("insertContent", new string[] { obj });
                    await WebView.InvokeScriptAsync("scrollToBottom", new string[] { scrollToEnd.ToString() });
                });
            });
        }

        public async static void RunActionOnUiThread(DispatchedHandler action)
        {
            if (dispatcher != null)
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, action);
            else
                action.Invoke();
        }

        public static string GetInfoString(string key)
        {
            return infoloader.GetString(key);
        }

        public static string GetErrorString(string key)
        {
            return errorloader.GetString(key);
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            LeftPane.IsPaneOpen = !LeftPane.IsPaneOpen;
        }

        private void ShowFlyout(object sender)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            ((BIRCViewModel)DataContext).ServerSelection = senderElement;
            flyoutBase.ShowAt(senderElement);
        }

        private void serverStackPanel_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ShowFlyout(sender);
        }

        private void CommandTxtBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            BIRCViewModel vm = ((BIRCViewModel)DataContext);

            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                e.Handled = true;
                vm.CommandTxt = CommandTxtBox.Text;
                vm.CommandAction();
            }
            else if (e.Key == Windows.System.VirtualKey.Up)
            {
                e.Handled = true;
                vm.CommandTxt = vm.GetSelectedConnection().CommandHistory.UpHistory();
                CommandTxtBox.SelectionStart = vm.CommandTxt.Length;
            }
            else if (e.Key == Windows.System.VirtualKey.Down)
            {
                e.Handled = true;
                vm.CommandTxt = vm.GetSelectedConnection().CommandHistory.DownHistory();
                CommandTxtBox.SelectionStart = vm.CommandTxt.Length;
            }
        }

        private void serverListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ListView lv = sender as ListView;

            ((BIRCViewModel)currentDataContext).ServerSelection = lv.SelectedItem;
            UsersListView.SelectedItem = null;
        }
    }
}
