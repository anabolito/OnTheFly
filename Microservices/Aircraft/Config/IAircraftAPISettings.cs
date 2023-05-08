namespace AircraftAPI.Config
{
    public interface IAircraftAPISettings
    {
        string AircraftCollectionName { get; set; }
        string DeletedAircraftCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
