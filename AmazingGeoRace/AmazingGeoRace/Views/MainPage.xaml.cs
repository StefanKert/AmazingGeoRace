using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using AmazingGeoRace.Models;
using AmazingGeoRace.ViewModels;

namespace AmazingGeoRace.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            RoutesList.Items.Add(new Route {
                Name = "Test"
            });
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
