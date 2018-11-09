using System;

namespace BusBoard.ConsoleApp
{
    public class Bus
    {
        public string LineName;
        public string DestinationName;
        public DateTime ExpectedArrival;

        public Bus(string lineName, string destinationName, DateTime expectedArrival)
        {
            DestinationName = destinationName;
            ExpectedArrival = expectedArrival;
            LineName = lineName;
        }

        public override string ToString()
        {
            return $"Line Name: {LineName}, Destination Name: {DestinationName}, Expected Arrival: {ExpectedArrival.ToShortTimeString()}!!!!!!!";
        }
    }
}