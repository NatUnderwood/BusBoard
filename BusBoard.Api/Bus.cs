using System;

namespace BusBoard.Api
{
    public class Bus
    {
        public string LineName { get; private set; }
        public string DestinationName { get; private set; }
        public DateTime ExpectedArrival { get; private set; }

        public Bus(string lineName, string destinationName, DateTime expectedArrival)
        {
            DestinationName = destinationName;
            ExpectedArrival = expectedArrival;
            LineName = lineName;
        }

        public override string ToString()
        {
            return $"Line Name: {LineName}, Destination Name: {DestinationName}, Expected Arrival: {ExpectedArrival.ToShortTimeString()}!!";
        }
    }
}