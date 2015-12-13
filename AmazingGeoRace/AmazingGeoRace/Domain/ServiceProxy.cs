using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmazingGeoRace.Data;
using AmazingRaceService.Interface;

namespace AmazingGeoRace.Domain
{
    public class ServiceProxy
    {
        public async Task<bool> CheckCredentials(string username, string password) {
            return await WebService.QueryDataFromService<bool>($"/CheckCredentials?userName={username}&password={password}");
        }

        public async Task<IEnumerable<Route>> GetRoutes(string username, string password)
        {
            return await WebService.QueryDataFromService<IEnumerable<Route>>($"/GetRoutes?userName={username}&password={password}");
        }

        public async Task<bool> InformAboutVisitedCheckpoint(CheckpointRequest checkpoint)
        {
            return await WebService.PostDataToService("/InformAboutVisitedCheckpoint", checkpoint);
        }

        public async Task<bool> ResetAllRoutes(Request userData)
        {
            return await WebService.PostDataToService("/ResetAllRoutes", userData);
        }

        public async Task<bool> ResetRoute(RouteRequest routeToReset)
        {
            return await WebService.PostDataToService("/ResetRoute", routeToReset);
        }
    }
}
