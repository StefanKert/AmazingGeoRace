using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AmazingGeoRace.Common;
using AmazingGeoRace.Domain;

namespace AmazingGeoRace.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private LoginService LoginService { get; }
        private string _username;

        public string Username
        {
            get { return _username; }
            set { OnPropertyChanged(ref _username, value); }
        }

        public ICommand LoginCommand { get; set; }

        public LoginViewModel(LoginService loginService) {
            LoginService = loginService;
            LoginCommand = new RelayCommand(async obj => await OnLoginCommand(obj));
        }

        private async Task OnLoginCommand(object obj) {
            await ExceptionHandling.HandleException(async () => {
                var password = obj as string;
                if (string.IsNullOrEmpty(password)) {
                    await MessageBoxWrapper.ShowOkAsync("No password given for user.");
                }
                else {
                    await LoginService.Login(Username, password);
                    ((Frame)Window.Current.Content).Navigate(typeof(Views.MainPage));
                }
            });
        }
    }
}
