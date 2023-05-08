namespace AircraftAPI.Config
{
    public class AircraftAPISettings : IAircraftAPISettings
    {
        public string AircraftCollectionName { get ; set ; }
        public string DeletedAircraftCollectionName { get ; set ; }
        
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
