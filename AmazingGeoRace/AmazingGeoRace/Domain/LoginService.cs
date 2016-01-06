using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AmazingGeoRace.Models;

namespace AmazingGeoRace.Domain
{
    public class LoginService
    {
        public Credentials Credentials { get; set; }

        public bool IsAuthenticated() {
            return Credentials != null;
        }

        public async Task Login(string username, string password, Action onSucceeded, Action<Exception> onFailed) {
            var serviceProxy = new ServiceProxy();
            if (await serviceProxy.CheckCredentials(new Credentials(username, password))) {
                Credentials = new Credentials(username, password);
                onSucceeded();
            }
            else
                onFailed(new Exception($"Login with username {username} failed."));
        }

        public void Logout()
        {
            Credentials = null;
            Application.Current.Exit();
        }
    }
}
