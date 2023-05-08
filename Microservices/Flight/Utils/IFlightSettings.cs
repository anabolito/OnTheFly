namespace FlightAPI.Utils
{
    public interface IFlightSettings
    {
        string FlightsCollectionName { get; set; }
        string CanceledFlightsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DataBaseName { get; set; }
    }
}
