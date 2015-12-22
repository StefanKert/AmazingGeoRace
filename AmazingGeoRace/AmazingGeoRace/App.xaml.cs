using AmazingGeoRace.Domain;
using AmazingGeoRace.ViewModels;
using AmazingRaceService.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using AmazingGeoRace.Common;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace AmazingGeoRace
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        private TransitionCollection transitions;

        internal RaceDetailsViewModel RaceDetailsViewModel { get; private set; }
        internal LoginViewModel LoginViewModel { get; private set; }
        internal MainViewModel MainViewModel { get; private set; }


        public new static App Current { get { return (App)Application.Current; } }


        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            IntergrateViewModels();
            this.Suspending += this.OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                // Set the default language
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(LoginPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private void IntergrateViewModels()
        {
            var loginService = new LoginService();
            var serviceProxy = new ServiceProxy();
            LoginViewModel = new LoginViewModel();
            LoginViewModel.PerformLogin += async (username, password) => {
                await ExceptionHandling.HandleExceptionForAsyncMethod(async () => {
                    await loginService.Login(username, password);
                    ((Frame)Window.Current.Content).Navigate(typeof(MainPage));
                });
            };

            MainViewModel = new MainViewModel();
            MainViewModel.ReloadRoutes += async () => {
                await ExceptionHandling.HandleExceptionForAsyncMethod(async () =>  MainViewModel.SetRoutes(await serviceProxy.GetRoutes(loginService.Credentials.Username, loginService.Credentials.Password)));
            };
            
            MainViewModel.ResetAllRoutes += async () => {
                await ExceptionHandling.HandleExceptionForAsyncMethod(async () => {
                    await serviceProxy.ResetAllRoutes(new Request {
                        Password = loginService.Credentials.Password,
                        UserName = loginService.Credentials.Username
                    });
                    var dialog = new MessageDialog("Routes successfully resetted.");
                    await dialog.ShowAsync();
                });
            };
            MainViewModel.ShowRouteDetails += (route) => {
                ((Frame)Window.Current.Content).Navigate(typeof(RaceDetailsPage), route);
            };
            

            RaceDetailsViewModel = new RaceDetailsViewModel();
            RaceDetailsViewModel.ShowSuccessMessage += async succeededRoute =>
            {
                var dialog = new MessageDialog($"You finished {succeededRoute.Name} successfully! Congratulations.");
                await dialog.ShowAsync();
            };
            RaceDetailsViewModel.ShowUnlockCheckpointDialog += async checkPoint =>
            {
                var dialog = new SolutionDialog();
                await dialog.ShowAsync();
                await Dialog_TrySolution(dialog.Solution);
            };
            RaceDetailsViewModel.ResetRoute += async route => {
                var result = await serviceProxy.ResetRoute(new RouteRequest {
                    UserName = "s1310307019",
                    Password = "s1310307019",
                    RouteId = route.Id
                });
                if (result) {
                    var routes = await serviceProxy.GetRoutes("s1310307019", "s1310307019");
                    RaceDetailsViewModel.Route = routes.FirstOrDefault(x => x.Id == route.Id);
                }
                else {
                    var dialog = new MessageDialog($"An error occurred when trying to reset route {route.Name}.");
                    await dialog.ShowAsync();
                }

            };
        }

        private async Task Dialog_TrySolution(string solution)
        {
            await CheckSolutionForCheckPoint(solution, RaceDetailsViewModel.NextCheckPoint, async () =>
            {
                var serviceProxy = new ServiceProxy();
                var routes = await serviceProxy.GetRoutes("s1310307019", "s1310307019");
                RaceDetailsViewModel.Route = routes.FirstOrDefault(x => x.Id == RaceDetailsViewModel.Route.Id);
            }, async () =>
            {
                var dialog = new MessageDialog($"{solution} wasn´t the correct solution. Please try anotherone.");
                await dialog.ShowAsync();
            });

        }

        private async Task CheckSolutionForCheckPoint(string solution, Checkpoint checkPoint, Action onCorrect, Action onIncorrect)
        {
            var serviceProxy = new ServiceProxy();
            var result = await serviceProxy.InformAboutVisitedCheckpoint(new CheckpointRequest
            {
                CheckpointId = checkPoint.Id,
                UserName = "s1310307019",
                Password = "s1310307019",
                Secret = solution
            });
            if (result)
                onCorrect();
            else
                onIncorrect();
        }
    }
}