using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using AmazingGeoRace.Common;
using AmazingGeoRace.Domain;
using AmazingRaceService.Interface;

namespace AmazingGeoRace.ViewModels
{
    public class MainViewModel
    {
        public ICommand ShowRouteDetailsCommand { get; set; }
        public ICommand ResetAllRoutesCommand { get; set; }
        public ObservableCollection<Route> Routes { get; } = new ObservableCollection<Route>();

        public MainViewModel() {
            ShowRouteDetailsCommand = new RelayCommand(OnShowRouteDetailsCommand);
            ResetAllRoutesCommand = new RelayCommand(obj => OnResetAllRoutesCommand());
        }


        public void Initialize()
        {
            ReloadRoutes();
        }

        public void SetRoutes(IEnumerable<Route> routes) {
            Routes.Clear();
            foreach (var route in routes) {
                Routes.Add(route);
            }
        }

        private async void OnShowRouteDetailsCommand(object obj)
        {
            await ExceptionHandling.HandleException(() => {
                var route = obj as Route;
                if (route == null)
                    throw new Exception("No route selected.");
                ShowRouteDetails(route);
            });
        }

        private async void OnResetAllRoutesCommand() {
            await ExceptionHandling.HandleException(() => { ResetAllRoutes(); });
        }

        public event Action ReloadRoutes;
        public event Action ResetAllRoutes;
        public event Action<Route> ShowRouteDetails;
    }
}
