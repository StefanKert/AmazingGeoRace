﻿using AmazingRaceService.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AmazingGeoRace.Data;
using AmazingGeoRace.ViewModels;
using AmazingGeoRace.Common;
using AmazingGeoRace.Domain;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace AmazingGeoRace
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var mainViewModel = new MainViewModel();
            await mainViewModel.LoadRoutes();
            DataContext = mainViewModel;
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RoutesList.SelectedItem != null) {
                Frame.Navigate(typeof(RaceDetailsPage), RoutesList.SelectedItem);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var serviceProxy = new ServiceProxy();
            await serviceProxy.ResetAllRoutes(new Request
            {
                Password = "s1310307019",
                UserName = "s1310307019"
            });
        }
    }
}
