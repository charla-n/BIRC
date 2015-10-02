﻿using BIRC.Shared.Models;
using BIRC.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        private static CoreDispatcher dispatcher;
        private static ResourceLoader loader;

        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            loader = ResourceLoader.GetForCurrentView(App.RESOURCE_NAME);
        }

        public async static void RunActionOnUiThread(DispatchedHandler action)
        {
            if (dispatcher != null)
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, action);
            else
                action.Invoke();
        }

        public static string GetString(string key)
        {
            return loader.GetString(key);
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
    }
}
