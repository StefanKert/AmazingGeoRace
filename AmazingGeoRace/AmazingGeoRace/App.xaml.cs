using AmazingGeoRace.Domain;
using AmazingGeoRace.ViewModels;
using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using AmazingGeoRace.Common;

namespace AmazingGeoRace
{
    public sealed partial class App
    {
        private TransitionCollection _transitions;
        private readonly LoginService _loginService;

        internal RaceDetailsViewModel RaceDetailsViewModel { get; private set; }
        internal LoginViewModel LoginViewModel { get; private set; }
        internal MainViewModel MainViewModel { get; private set; }


        public new static App Current => (App)Application.Current;

        public App()
        {
            InitializeComponent();
            _loginService = new LoginService();
            var serviceProxy = new ServiceProxy();
            LoginViewModel = new LoginViewModel(_loginService);
            MainViewModel = new MainViewModel(serviceProxy, _loginService);
            RaceDetailsViewModel = new RaceDetailsViewModel(serviceProxy, _loginService);
            Suspending += OnSuspending;
        }

        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            if (rootFrame == null)
                return;
            rootFrame.ContentTransitions = _transitions ?? new TransitionCollection { new NavigationThemeTransition() };
            rootFrame.Navigated -= RootFrame_FirstNavigated;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e) {
#if DEBUG
            if (Debugger.IsAttached) {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null) {
                rootFrame = new Frame {
                    CacheSize = 1,
                    Language = Windows.Globalization.ApplicationLanguages.Languages[0]
                };

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) {
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                        if ((bool) SuspensionManager.SessionState["Authenticated"]) {
                            var username = SuspensionManager.SessionState["Username"] as string;
                            var password = SuspensionManager.SessionState["Password"] as string;
                            await _loginService.Login(username, password);
                        }
                        await SuspensionManager.RestoreAsync("frameSessionKey");
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Session restore failed.");
                    }
                }
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null) {
                if (rootFrame.ContentTransitions != null) {
                    _transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions) {
                        _transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += RootFrame_FirstNavigated;

                if (!rootFrame.Navigate(typeof(Views.LoginPage), e.Arguments)) {
                    throw new Exception("Failed to create initial page");
                }
            }
            Window.Current.Activate();
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            try {
                SuspensionManager.SessionState["Authenticated"] = _loginService.IsAuthenticated();
                if (_loginService.IsAuthenticated()) {
                    SuspensionManager.SessionState["Username"] = _loginService.Credentials?.Username;
                    SuspensionManager.SessionState["Password"] = _loginService.Credentials?.Password;
                }
                await SuspensionManager.SaveAsync();
            }
            catch (Exception)
            {
                Debug.WriteLine("Saving session failed.");
            }
            deferral.Complete();
        }   
    }
}