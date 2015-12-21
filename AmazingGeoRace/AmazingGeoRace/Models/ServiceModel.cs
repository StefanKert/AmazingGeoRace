using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Windows.Devices.Geolocation;

namespace AmazingRaceService.Interface {
	[DataContract]
	public class Route {
		[DataMember]
		public Guid Id { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public Checkpoint[] VisitedCheckpoints { get; set; }
		[DataMember]
		public Checkpoint NextCheckpoint { get; set; }
	}

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

	[DataContract]
	public class Request {
		[DataMember]
		public string UserName { get; set; }
		[DataMember]
		public string Password { get; set; }
	}

	[DataContract]
	public class CheckpointRequest : Request {
		[DataMember]
		public Guid CheckpointId { get; set; }
		[DataMember]
		public string Secret { get; set; }
	}

	[DataContract]
	public class RouteRequest : Request {
		[DataMember]
		public Guid RouteId { get; set; }
	}
}
