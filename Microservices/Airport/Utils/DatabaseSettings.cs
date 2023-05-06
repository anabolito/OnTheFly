namespace AirportAPI.Utils
{
    public class DataBaseSettings : IDataBaseSettings
    {
        public string AirportCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DataBaseName { get; set; }
    }

}
