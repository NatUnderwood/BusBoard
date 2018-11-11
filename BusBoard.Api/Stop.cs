using System.Runtime.InteropServices;

namespace BusBoard.Api
{
    public class Stop
    {
        public string CommonName { get; private set; }
        public string Indicator { get; private set; }
        public string Id { get; private set; }
        public double Distance { get; private set; }

        public Stop(string commonName, string indicator, string id, double distance)
        {
            CommonName = commonName;
            Indicator = indicator;
            Id = id;
            Distance = distance;
        }

        public override string ToString()
        {
            return $"{CommonName} {Indicator}";
        }
    }
}