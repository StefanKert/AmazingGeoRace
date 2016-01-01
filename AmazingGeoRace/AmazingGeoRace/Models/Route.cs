using System;
using System.Runtime.Serialization;

namespace AmazingGeoRace.Models
{
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
}