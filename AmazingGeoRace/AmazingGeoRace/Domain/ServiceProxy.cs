using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmazingGeoRace.Data;
using AmazingGeoRace.Models;

namespace AmazingGeoRace.Domain
{
    public class ServiceProxy
    {
        public async Task<bool> CheckCredentials(Credentials credentials) {
            return await WebService.QueryDataFromService<bool>($"/CheckCredentials?userName={credentials.Username}&password={credentials.Password}");
        }

        public async Task<IEnumerable<Route>> GetRoutes(Credentials credentials)
        {
            return await WebService.QueryDataFromService<IEnumerable<Route>>($"/GetRoutes?userName={credentials.Username}&password={credentials.Password}");
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
