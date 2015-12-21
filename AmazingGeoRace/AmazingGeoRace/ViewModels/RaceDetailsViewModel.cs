using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
using AmazingGeoRace.Common;
using AmazingGeoRace.Domain;
using AmazingRaceService.Interface;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

namespace AmazingGeoRace.ViewModels
{
    public class RaceDetailsViewModel: ViewModelBase
    {
        public ICommand ShowUnlockCheckpointDialogCommand { get; set; }
        public ICommand ResetRouteCommand { get; set; }

        private bool _finished;
        public bool Finished
        {

            get { return _finished; }
            set
            {
                if (Equals(value, _finished))
                    return;
                _finished = value;
                OnPropertyChanged();
            }
        }

        private Route _route;
        public Route Route
        {
            get { return _route; }
            set
            {
                if (Equals(value, _route))
                    return;
                _route = value;
                OnPropertyChanged();
                if (_route.NextCheckpoint != null)
                    OnPropertyChanged(nameof(NextCheckPoint));
                else {
                    ShowSuccessMessage(value);
                    Finished = true;
                }
            }
        }

        public RaceDetailsViewModel() {
            ShowUnlockCheckpointDialogCommand = new RelayCommand(obj => ShowUnlockCheckpointDialog(NextCheckPoint), () => !Finished);
            ResetRouteCommand = new RelayCommand(obj => ResetRoute(Route), () => Finished);
        }

        public Checkpoint NextCheckPoint => Route.NextCheckpoint;

        public List<MapElement> GetMapElementsForCurrentRoute() {
            var elements = new List<MapElement>();
            var checkPoints = Route.VisitedCheckpoints.ToList();
            if (Route.NextCheckpoint != null)
            {
                elements.Add(GetMapIconForCheckPoint(Route.NextCheckpoint));
                checkPoints.Add(Route.NextCheckpoint);
            }
            elements.AddRange(Route.VisitedCheckpoints.Select(GetMapIconForCheckPoint));
            elements.Add(GetLinesForCheckPoints(checkPoints));
            return elements;
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

        public event Action<MapElement> AddMapElement;
        public event Action<Route> ShowSuccessMessage;
        public event Action<Route> ResetRoute;
        public event Action<Checkpoint> ShowUnlockCheckpointDialog;

    }
}
