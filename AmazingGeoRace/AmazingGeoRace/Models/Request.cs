using System.Runtime.Serialization;
using AmazingGeoRace.Domain;

namespace AmazingGeoRace.Models
{
    [DataContract]
    public class Request {
        [DataMember]
        public string UserName { get; private set; }
        [DataMember]
        public string Password { get; private set; }

        public Request(Credentials credentials) {
            UserName = credentials.Username;
            Password = credentials.Password;
        }
    }
}