using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using AmazingGeoRace.Common;
using AmazingGeoRace.Domain;

namespace AmazingGeoRace.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;

        public string Username
        {
            get { return _username; }
            set
            {
                if (Equals(value, _username))
                    return;
                _username = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; set; }

        public LoginViewModel() {
            LoginCommand = new RelayCommand(async obj => await OnLoginCommand(obj));
        }

        private async Task OnLoginCommand(object obj) {
#if DEBUG
            LoginSucceeded();
            return;
#endif
            var password = obj as string;
            if (string.IsNullOrEmpty(password))
            {
                var dialog = new MessageDialog("No password given for user.", "Error");
                await dialog.ShowAsync();
                return;
            }
            else {
                var serviceProxy = new ServiceProxy();
                if (await serviceProxy.CheckCredentials(Username, password))
                    LoginSucceeded();
                else {
                    var dialog = new MessageDialog("Login failed. Please check your credentials.", "Error");
                    await dialog.ShowAsync();
                }
            }

        }

        public event Action LoginSucceeded;
    }
}
