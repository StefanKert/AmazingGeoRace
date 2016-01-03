using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AmazingGeoRace.Domain
{
    public class Credentials
    {
        public string Username { get; }
        public string Password { get; }

        public Credentials(string username, string password) {
            Username = username;
            Password = password;
        }
    }

    public class LoginService
    {
        public Credentials Credentials { get; set; }

        public bool IsAuthenticated() {
            return Credentials != null;
        }

        public async Task Login(string username, string password, Action onSucceeded) {
            var serviceProxy = new ServiceProxy();
            if (await serviceProxy.CheckCredentials(new Credentials(username, password))) {
                Credentials = new Credentials(username, password);
                onSucceeded();
            }
            else
                throw new Exception($"Login with username {username} failed.");
        }
    }
}
