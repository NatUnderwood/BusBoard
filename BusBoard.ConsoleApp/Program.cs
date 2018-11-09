using System;
using System.Collections.Generic;
using System.Dynamic;
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
            var listOfBuses = new List<BusJson>();
            double lat, lon;
            Stop[] stops;


            Console.WriteLine("Enter your postcode:");
            var postcode = Console.ReadLine();
            ApiCaller.RetrieveLatLongfromPostcode(postcode, out lat, out lon);
            ApiCaller.RetrieveNearestStopsFromLatLong(lat, lon, out stops);
            foreach (Stop stop in stops)
            {
                Console.WriteLine(stop.ToString());
                ApiCaller.RetrieveBusList(stop.Id, out listOfBuses);
                listOfBuses.RemoveRange(5, listOfBuses.Count - 5);
                foreach (var bus in listOfBuses)
                {
                    Console.WriteLine(bus.ToBus().ToString());
                }
            }
        }
    }
}
