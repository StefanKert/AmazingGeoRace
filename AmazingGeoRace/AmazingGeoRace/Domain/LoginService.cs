using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazingGeoRace.Domain
{
    public class Credentials
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public Credentials(string username, string password) {
            Username = username;
            Password = password;
        }
    }

    public class LoginService
    {
        public Credentials Credentials { get; set; }

        public async Task Login(string username, string password) {
#if DEBUG
            Credentials = new Credentials("s1310307019", "s1310307019");
            return;
#endif
            var serviceProxy = new ServiceProxy();
            if (await serviceProxy.CheckCredentials(username, password)) 
                Credentials = new Credentials(username, password);
            else 
                throw new Exception($"Login with username {username} failed.");
        }
    }
}
