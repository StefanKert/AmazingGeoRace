using System.Diagnostics;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using AmazingGeoRace.Common;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace AmazingGeoRace.Views
{
    public sealed partial class LoginPage: Page
    {
        public LoginPage() {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }


        protected override void OnNavigatedTo(NavigationEventArgs e) {
            var viewmodel = App.Current.LoginViewModel;
            DataContext = viewmodel;
        }
    }
}