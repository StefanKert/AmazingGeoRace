using System;
using System.Runtime.Serialization;
using Windows.Devices.Geolocation;

namespace AmazingGeoRace.Models
{
    [DataContract]
    public class Checkpoint
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Hint { get; set; }
        [DataMember]
        public decimal Latitude { get; set; }
        [DataMember]
        public decimal Longitude { get; set; }

        public Geopoint Location => new Geopoint(new BasicGeoposition
        {
            Latitude = (double)Latitude,
            Longitude = (double)Longitude
        });
    }
}