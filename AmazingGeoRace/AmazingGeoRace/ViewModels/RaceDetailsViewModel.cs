﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using AmazingGeoRace.Common;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using AmazingGeoRace.Commands;
using AmazingGeoRace.Domain;
using AmazingGeoRace.Models;

namespace AmazingGeoRace.ViewModels
{
    public class RaceDetailsViewModel: ViewModelBase
    {
        public ServiceProxy ServiceProxy { get; set; }
        public LoginService LoginService { get; set; }
        public ICommand ShowUnlockCheckpointDialogCommand { get; set; }
        public ICommand ResetRouteCommand { get; set; }

        private bool _finished;
        public bool Finished
        {

            get { return _finished; }
            set { OnPropertyChanged(ref _finished, value); }
        }

        private Route _route;
        public Route Route
        {
            get { return _route; }
            private set { OnPropertyChanged(ref _route, value); }
        }

        public Checkpoint NextCheckPoint => Route.NextCheckpoint;

        public MapControl Map { get; set; }

        public RaceDetailsViewModel(ServiceProxy serviceProxy, LoginService loginService) {
            ServiceProxy = serviceProxy;
            LoginService = loginService;
            ShowUnlockCheckpointDialogCommand = new RelayCommand(obj => ShowUnlockCheckpointDialog());
            ResetRouteCommand = new RelayCommand(obj => ResetRoute(Route));
        }

        public void SetRoute(Route route)
        {
            Route = route;
            RefreshMap(GetMapElementsForCurrentRoute(), Route);
            if (Route.NextCheckpoint != null)
            {
                OnPropertyChanged("NextCheckPoint");
                Finished = false;
            }
            else {
                ShowSuccessMessage(route);
                Finished = true;
            }
        }

        public List<MapElement> GetMapElementsForCurrentRoute() {
            var elements = new List<MapElement>();
            var checkPoints = Route.VisitedCheckpoints.ToList();
            if (Route.NextCheckpoint != null) {
                elements.Add(GetMapIconForCheckPoint(Route.NextCheckpoint));
                checkPoints.Add(Route.NextCheckpoint);
            }
            elements.AddRange(Route.VisitedCheckpoints.Select(GetMapIconForCheckPoint));
            elements.Add(GetLinesForCheckPoints(checkPoints));
            return elements;
        }

        public void RefreshMap(IEnumerable<MapElement> elements, Route route) {
            Map.MapElements.Clear();
            foreach (var element in elements) {
                Map.MapElements.Add(element);
            }
            Map.Center = route.NextCheckpoint?.Location ?? route.VisitedCheckpoints.Last()?.Location;
        }

        private MapPolyline GetLinesForCheckPoints(IEnumerable<Checkpoint> checkPoints)
        {
            return new MapPolyline
            {
                Path = new Geopath(checkPoints.Select(x => new BasicGeoposition
                {
                    Latitude = (double)x.Latitude,
                    Longitude = (double)x.Longitude
                })),
                StrokeColor = Colors.Red,
                StrokeThickness = 5            
            };
        }

        private MapIcon GetMapIconForCheckPoint(Checkpoint checkPoint)
        {
            return new MapIcon()
            {
                Location = checkPoint.Location,
                Title = checkPoint.Name,
                NormalizedAnchorPoint = new Windows.Foundation.Point(0.5, 0.95),
                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/mappin.png"))
            };
        }

        private async void ResetRoute(Route route) {
            var result = await ServiceProxy.ResetRoute(new RouteRequest(LoginService.Credentials, route.Id));
            if (result) {
                var routes = await ServiceProxy.GetRoutes(LoginService.Credentials);
                SetRoute(routes.FirstOrDefault(x => x.Id == route.Id));
                await MessageBoxWrapper.ShowOkAsync("Resetting route " + route.Name + " successful.");
                Finished = false;
            }
            else {
                await MessageBoxWrapper.ShowOkAsync("An error occurred when trying to reset route " + route.Name +".");
            }
        }

        private async void ShowSuccessMessage(Route route) {
            await MessageBoxWrapper.ShowOkAsync("You finished " + route.Name + " successfully! Congratulations.");
        }

        private async void ShowUnlockCheckpointDialog() {
            var dialog = new Views.SolutionDialog();
            var contentResult = await dialog.ShowAsync();
            if (contentResult == ContentDialogResult.Primary) {
                var result = await ServiceProxy.InformAboutVisitedCheckpoint(new CheckpointRequest(LoginService.Credentials, NextCheckPoint.Id, dialog.Solution));
                if (result) {
                    await MessageBoxWrapper.ShowOkAsync("Congratulations. Correct answer!");
                    var routes = await ServiceProxy.GetRoutes(LoginService.Credentials);
                    SetRoute(routes.FirstOrDefault(x => x.Id == Route.Id));
                }
                else {
                    await MessageBoxWrapper.ShowOkAsync(dialog.Solution + " was not the correct solution. Please try anotherone.");
                }
            }
        }
    }
}
