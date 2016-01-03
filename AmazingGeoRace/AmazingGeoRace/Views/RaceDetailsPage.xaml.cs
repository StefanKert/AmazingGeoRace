using System.Collections.Generic;
using System.Linq;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;
using AmazingGeoRace.Models;

namespace AmazingGeoRace.Views
{
    public sealed partial class RaceDetailsPage: Page
    {

        public RaceDetailsPage() {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e) {
            if (!Frame.CanGoBack)
                return;
            e.Handled = true;
            Frame.GoBack();
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            var route = e.Parameter as Route;
            if (route == null) {
                Frame.Navigate(e.SourcePageType);
                return;
            }

            var viewModel = App.Current.RaceDetailsViewModel;
            viewModel.Map = Map;
            viewModel.SetRoute(route);
            DataContext = viewModel;
        }
    }
}
