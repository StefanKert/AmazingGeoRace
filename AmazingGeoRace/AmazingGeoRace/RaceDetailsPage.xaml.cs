using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AmazingGeoRace.ViewModels;
using AmazingRaceService.Interface;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Controls.Maps;
using AmazingGeoRace.Domain;
using Windows.UI.Popups;
using System.Threading.Tasks;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace AmazingGeoRace
{
    public sealed partial class RaceDetailsPage : Page
    {

        public RaceDetailsPage()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (!Frame.CanGoBack)
                return;
            e.Handled = true;
            Frame.GoBack();
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            var route = e.Parameter as Route;
            if (route == null) {
                Frame.Navigate(e.SourcePageType);
                return;
            }

            var viewModel = App.Current.RaceDetailsViewModel;
            viewModel.Route = route;
            var elements = viewModel.GetMapElementsForCurrentRoute();
            foreach (var element in elements) {
                Map.MapElements.Add(element);
            }
            Map.Center = route.NextCheckpoint?.Location ?? route.VisitedCheckpoints.Last()?.Location;
            DataContext = viewModel;
        }
    }
}
