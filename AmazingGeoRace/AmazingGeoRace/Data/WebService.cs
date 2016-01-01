using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace AmazingGeoRace.Data {

    public class WebserviceCallException: Exception
    {
        public WebserviceCallException() {}

        public WebserviceCallException(string message): base(message) {}

        public WebserviceCallException(string message, Exception innerException): base(message, innerException) {}
    }

    public static class WebService {
	    private const string ServiceUrl = "https://demo.nexperts.com/MOC5/AmazingRaceService/AmazingRaceService.svc{0}";
		
		public static async Task<T> QueryDataFromService<T>(string endpoint) {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { MaxAge = TimeSpan.Zero };
            var result = await httpClient.GetAsync(string.Format(ServiceUrl, endpoint));
            var content = await result.Content.ReadAsStringAsync();
		    if (result.IsSuccessStatusCode)
		        return JsonConvert.DeserializeObject<T>(content);
		    throw new WebserviceCallException(content);

		}

		public static async Task<bool> PostDataToService<TRequest>(string endpoint, TRequest request) {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { MaxAge = TimeSpan.Zero };
            var result = await httpClient.PostAsJsonAsync(string.Format(ServiceUrl, endpoint), request);
            var content = await result.Content.ReadAsStringAsync();
		    if (result.IsSuccessStatusCode)
		        return JsonConvert.DeserializeObject<bool>(content);
            throw new WebserviceCallException(content);
        }
	}
}
