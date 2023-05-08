namespace FlightAPI.Utils
{
    public class FlightSettings : IFlightSettings
    {
        public string ConnectionString { get; set; }
        public string DataBaseName { get; set; }
        public string FlightsCollectionName { get; set; }
        public string CanceledFlightsCollectionName { get; set; }
        public string DeletedFlightsCollectionName { get; set; }
    }
}
