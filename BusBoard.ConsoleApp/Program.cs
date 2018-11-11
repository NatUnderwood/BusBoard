using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using BusBoard.Api;

namespace BusBoard.ConsoleApp

{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Console.WriteLine("Enter your postcode:");
            var postcode = Console.ReadLine();
            List<string> busInfo = ApiCaller.GetBusInfoPostcode(postcode);
            foreach (string line in busInfo)
                Console.WriteLine(line);
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
