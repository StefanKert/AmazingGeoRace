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
            await ExceptionHandling.HandleException(() => {
                var password = obj as string;
                if (string.IsNullOrEmpty(password)) {
                    throw new Exception("No password given for user.");
                }
                PerformLogin(Username, password);
            });
        }

        public event Action<string, string> PerformLogin;
    }
}
