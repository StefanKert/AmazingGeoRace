using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AmazingGeoRace.Views
{
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
