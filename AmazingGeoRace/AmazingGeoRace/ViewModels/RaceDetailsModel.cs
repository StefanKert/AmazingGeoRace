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

namespace AmazingGeoRace.ViewModels
{
    public class RaceDetailsViewModel : ViewModelBase
    {
        private Route _route;
        public Route Route
        {
            get { return _route; }
            set
            {
                if (Equals(value, _route))
                    return;
                _route = value;
                NextCheckPoint = _route.NextCheckpoint;
                OnPropertyChanged();
            }
        }

        private Checkpoint _nextCheckpoint;
        public Checkpoint NextCheckPoint
        {
            get { return _nextCheckpoint; }
            set
            {
                if (Equals(value, _nextCheckpoint))
                    return;
                _nextCheckpoint = value;
                NextCheckPointLocation = new Geopoint(new BasicGeoposition() {
                    Longitude = (double) value.Longitude,
                    Latitude = (double) value.Latitude
                });
                OnPropertyChanged();
            }
        }

        private Geopoint _nextCheckPointLocation;
        public Geopoint NextCheckPointLocation
        {
            get { return _nextCheckPointLocation; }
            set
            {
                if (Equals(value, _nextCheckPointLocation))
                    return;
                _nextCheckPointLocation = value;
                OnPropertyChanged();
            }
        }

        public RaceDetailsViewModel(Route route) {
            Route = route;
        }
    }
}
