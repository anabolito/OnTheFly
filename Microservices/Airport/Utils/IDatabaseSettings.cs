namespace AirportAPI.Utils
{
    public interface IDataBaseSettings
    {
        string AirportCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DataBaseName { get; set; }
    }
}