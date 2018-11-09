using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace BusBoard.ConsoleApp
{
    public static class ApiCaller
    {



        

        public static bool RetrieveBusList(string stopId, out List<BusJson> ListBusJson)
        {
            RestClient TflClient = new RestClient("https://api.tfl.gov.uk/");
            RestRequest busRequest = new RestRequest("/StopPoint/{id}/Arrivals", Method.GET);
            busRequest.AddUrlSegment("id", stopId);
            try
            {
                IRestResponse response = TflClient.Execute(busRequest);
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
            RestClient postcodeClient = new RestClient("https://api.postcodes.io/");
            RestRequest postcodeRequest = new RestRequest("/postcodes/{postcode}", Method.GET);
            postcodeRequest.AddUrlSegment("postcode", postcode);
            try
            {
                IRestResponse response = postcodeClient.Execute(postcodeRequest);
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

        //public static bool RetrieveNearestStopsFromLatLong(double latitude, double longitude, out string[] stops)
        //{
        //    RestClient TflClient = new RestClient("https://api.tfl.gov.uk/");
        //    RestRequest busRequest = new RestRequest("/StopPoint?stopTypes={stopTypes}&radius={radius}&modes={modes}&location.lat={lat}&location.lon={lon}", Method.GET);
        //}

    }
}