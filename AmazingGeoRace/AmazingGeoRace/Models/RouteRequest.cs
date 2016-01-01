using System;
using System.Runtime.Serialization;
using AmazingGeoRace.Domain;

namespace AmazingGeoRace.Models {

    [DataContract]
	public class RouteRequest : Request {
		[DataMember]
		public Guid RouteId { get; private set; }

	    public RouteRequest(Credentials credentials, Guid routeId): base(credentials) {
	        RouteId = routeId;
	    }
	}
}
