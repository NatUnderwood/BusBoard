using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace BusBoard.Api
{
    public static class ApiCaller
    {
        private static readonly RestClient TflClient = new RestClient("https://api.tfl.gov.uk/");
        private static readonly RestRequest BusRequest = new RestRequest("/StopPoint/{id}/Arrivals", Method.GET);
        private static readonly RestClient PostcodeClient = new RestClient("https://api.postcodes.io/");
        private static readonly RestRequest PostcodeRequest = new RestRequest("/postcodes/{postcode}", Method.GET);
        public static bool RetrieveBusList(string stopId, out List<BusJson> ListBusJson)
        {
            BusRequest.AddUrlSegment("id", stopId);
            try
            {
                IRestResponse response = TflClient.Execute(BusRequest);
                var content = response.Content;
                ListBusJson = JsonConvert.DeserializeObject<List<BusJson>>(content);
            }
            catch
            {
                ListBusJson = null;
                return false;
            }

            return true;
        }

        public static bool RetrieveLatLongfromPostcode(string postcode, out double latitude, out double longitude)
        {
            PostcodeRequest.AddUrlSegment("postcode", postcode);
            try
            {
                IRestResponse response = PostcodeClient.Execute(PostcodeRequest);
                var content = response.Content;
                var output = JObject.Parse(content);
                latitude = (double)output.GetValue("result")["latitude"];
                longitude = (double)output.GetValue("result")["longitude"];
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                latitude = 0;
                longitude = 0;
                return false;
            }
            return true;
        }

        public static bool RetrieveNearestStopsFromLatLong(double latitude, double longitude, out Stop[] stops)
        {
            RestRequest stopPointQuery = new RestRequest("/StopPoint?stopTypes={stopTypes}&radius={radius}&modes={modes}&lat={lat}&lon={lon}", Method.GET);
            stopPointQuery.AddUrlSegment("stopTypes", "NaptanPublicBusCoachTram");
            stopPointQuery.AddUrlSegment("radius", "1000" );
            stopPointQuery.AddUrlSegment("modes", "bus");
            stopPointQuery.AddUrlSegment("lat", latitude);
            stopPointQuery.AddUrlSegment("lon", longitude);
            try
            {
                IRestResponse response = TflClient.Execute(stopPointQuery);
                var content = response.Content;
                var output = JObject.Parse(content);
                var jStops = output.GetValue("stopPoints");
                stops = new Stop[2];

                for (int i = 0; i < 2; i ++)
                    stops[i] = new Stop((string)jStops[i]["commonName"], (string)jStops[i]["indicator"], (string)jStops[i]["id"]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                stops = null;
                return false;
            }
            return true;
        }
        

    }
}