using System;
using System.Runtime.Serialization;
using AmazingGeoRace.Domain;

namespace AmazingGeoRace.Models
{
    [DataContract]
    public class CheckpointRequest : Request {
        [DataMember]
        public Guid CheckpointId { get; private set; }
        [DataMember]
        public string Secret { get; private set; }

        public CheckpointRequest(Credentials credentials, Guid checkpointId, string secret): base(credentials) {
            CheckpointId = checkpointId;
            Secret = secret;
        }
    }
}