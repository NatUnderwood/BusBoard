using System;
using System.Collections.Generic;
using System.Linq;
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

        public static List<string> GetBusInfoPostcode(string postcode)
        {
            List<string> busInfo = new List<string>();
            if(!ApiCaller.RetrieveLatLongfromPostcode(postcode, out double lat, out double lon))
                return new List<string> {$"Please enter a valid postcode"};
                
            if(!ApiCaller.RetrieveNearestStopsFromLatLong(lat, lon, out List<Stop> stops))
                return new List<string> {$"London bus stops not found near {postcode}" };

            var nearestTwoStops = stops.OrderBy(i => i.Distance).Take(2);
            foreach (var stop in nearestTwoStops)
            {
                busInfo.Add(stop.ToString());
                ApiCaller.RetrieveBusList(stop.Id, out List<Bus> busList);
                var nextFiveBuses = busList.OrderBy(i => i.ExpectedArrival).Take(5);
                foreach (var bus in nextFiveBuses)
                    busInfo.Add(bus.ToString());
            }

            return busInfo;
        }

        public static bool RetrieveBusList(string stopId, out List<Bus> busList)
        {
            busList = new List<Bus>();
            BusRequest.AddUrlSegment("id", stopId);
            try
            {
                IRestResponse response = TflClient.Execute(BusRequest);
                var jsonBusList = JArray.Parse(response.Content);
                foreach (var jsonBus in jsonBusList)
                {
                    var lineName = (string)jsonBus["lineName"];
                    var destinationName = (string)jsonBus["destinationName"];
                    var expectedArrival = (DateTime) jsonBus["expectedArrival"];
                    busList.Add(new Bus(lineName, destinationName, expectedArrival));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                busList = null;
                return false;
            }

            return true;
        }

        public static bool RetrieveLatLongfromPostcode(string postcode, out double latitude, out double longitude)
        {
            PostcodeRequest.AddUrlSegment("postcode", postcode);
            try
            {
                var response = PostcodeClient.Execute(PostcodeRequest);
                var output = JObject.Parse(response.Content);
                latitude = (double) output.GetValue("result")["latitude"];
                longitude = (double) output.GetValue("result")["longitude"];
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
                latitude = 0;
                longitude = 0;
                return false;
            }
            return true;
        }

        public static bool RetrieveNearestStopsFromLatLong(double latitude, double longitude, out List<Stop> stops)
        {
            var stopPointQuery = new RestRequest("/StopPoint?stopTypes={stopTypes}&radius={radius}&modes={modes}&lat={lat}&lon={lon}", Method.GET);
            stopPointQuery.AddUrlSegment("stopTypes", "NaptanPublicBusCoachTram");
            stopPointQuery.AddUrlSegment("radius", "1000" );
            stopPointQuery.AddUrlSegment("modes", "bus");
            stopPointQuery.AddUrlSegment("lat", latitude);
            stopPointQuery.AddUrlSegment("lon", longitude);
            try
            {
                IRestResponse response = TflClient.Execute(stopPointQuery);
                var output = JObject.Parse(response.Content);
                var jStops = output.GetValue("stopPoints");
                stops = new List<Stop>();
                foreach (var jStop in jStops)
                {
                    var commonName = (string)jStop["commonName"];
                    var indicator = (string)jStop["indicator"];
                    var id = (string)jStop["id"];
                    var distance = (double)jStop["distance"];
                    stops.Add(new Stop(commonName, indicator, id, distance));
                }
                
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