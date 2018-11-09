using System;

namespace BusBoard.Api
{
    public class BusJson
    {
        public BusJson()
        {

        }

        public Bus ToBus()
        {
            Bus bus = new Bus(lineName, destinationName, DateTime.Parse(expectedArrival));
            return bus;
        }

        public string id;
        public int operationType;
        public string vehicleId;
        public string naptanId;
        public string stationName;
        public string lineId;
        public string lineName;
        public string platformName;
        public string direction;
        public string destinationNaptanId;
        public string destinationName;
        public string timeStamp;
        public string timeToStation;
        public string currentLocation;
        public string towards;
        public string expectedArrival;
        public string timeToLive;
        public string modeName;
        public Timing timing;

    }

    public class Timing
    {
        public Timing()
        {

        }

        public string countDownServerAdjustment;
        public string source;
        public string insert;
        public string read;
        public string sent;
        public string recieved;
    }

 
}