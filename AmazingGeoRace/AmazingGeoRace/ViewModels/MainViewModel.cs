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
        public async Task LoadRoutes() {
            var proxy = new ServiceProxy();
            var routes = await proxy.GetRoutes("s1310307019", "s1310307019");
            Routes = new ObservableCollection<Route>(routes);
        }

        public ObservableCollection<Route> Routes { get; set; } 
    }
}
