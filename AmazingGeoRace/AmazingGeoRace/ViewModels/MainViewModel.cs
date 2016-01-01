using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AmazingGeoRace.Common;
using AmazingGeoRace.Domain;
using AmazingGeoRace.Models;

namespace AmazingGeoRace.ViewModels
{
    public class MainViewModel
    {
        private ServiceProxy ServiceProxy { get; }
        private LoginService LoginService { get; }
        public ICommand ShowRouteDetailsCommand { get; set; }
        public ICommand ResetAllRoutesCommand { get; set; }
        public ObservableCollection<Route> Routes { get; } = new ObservableCollection<Route>();

        public MainViewModel(ServiceProxy serviceProxy, LoginService loginService) {
            ServiceProxy = serviceProxy;
            LoginService = loginService;
            ShowRouteDetailsCommand = new RelayCommand(async obj => await OnShowRouteDetailsCommand(obj));
            ResetAllRoutesCommand = new RelayCommand(async obj => await OnResetAllRoutesCommand());
        }


        public async void Initialize() {
            await ExceptionHandling.HandleExceptionForAsyncMethod(async () => SetRoutes(await ServiceProxy.GetRoutes(LoginService.Credentials)));
        }

        public void SetRoutes(IEnumerable<Route> routes) {
            Routes.Clear();
            foreach (var route in routes) {
                Routes.Add(route);
            }
        }

        private async Task OnShowRouteDetailsCommand(object obj) {
            await ExceptionHandling.HandleException(() => {
                var route = obj as Route;
                if (route == null)
                    throw new Exception("No route selected.");
                ((Frame)Window.Current.Content).Navigate(typeof(Views.RaceDetailsPage), route);
            });
        }

        private async Task OnResetAllRoutesCommand() {
            await ExceptionHandling.HandleException(async () => {
                await ExceptionHandling.HandleExceptionForAsyncMethod(async () => {
                    await ServiceProxy.ResetAllRoutes(new Request(LoginService.Credentials));
                    await MessageBoxWrapper.ShowOkAsync("Routes successfully resetted.");
                });
            });
        }
    }
}
