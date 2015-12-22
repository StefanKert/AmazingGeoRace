using AmazingRaceService.Interface;
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
        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            var viewModel = App.Current.MainViewModel;
            DataContext = viewModel;
            viewModel.Initialize();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (RoutesList.SelectedItem != null)
                App.Current.MainViewModel.ShowRouteDetailsCommand.Execute(RoutesList.SelectedItem);
        }
    }
}
