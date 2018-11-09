namespace BusBoard.Api
{
    public class Stop
    {
        public string CommonName;
        public string Indicator;
        public string Id;

        public Stop(string commonName, string indicator, string id)
        {
            CommonName = commonName;
            Indicator = indicator;
            Id = id;
        }

        public override string ToString()
        {
            return $"{CommonName} {Indicator}";
        }
    }
}