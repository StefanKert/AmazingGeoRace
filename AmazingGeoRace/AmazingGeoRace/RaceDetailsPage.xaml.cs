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

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace AmazingGeoRace
{
    public sealed partial class RaceDetailsPage : Page
    {
        public RaceDetailsPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            var route = e.Parameter as Route;
            if (route == null) {
                Frame.Navigate(e.SourcePageType);
                return;
            }
                
            var viewModel = new RaceDetailsViewModel(route);
            DataContext = viewModel;
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }
    }
}
