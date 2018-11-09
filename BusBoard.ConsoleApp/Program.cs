﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;

namespace BusBoard.ConsoleApp

{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new RestClient("https://api.tfl.gov.uk/");
            var request = new RestRequest("/StopPoint/{id}/Arrivals", Method.GET);
            request.AddUrlSegment("id", "490008660N");
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            var listOfBuses=JsonConvert.DeserializeObject<List<BusJson>>(content);
            listOfBuses.RemoveRange(5, listOfBuses.Count - 5);
            foreach (var bus in listOfBuses)
            {
                Console.WriteLine(bus.ToBus().ToString());
            }
        }
    }
}
