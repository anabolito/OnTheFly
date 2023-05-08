namespace PassengerAPI.Utils
{
    public class PassengerSettings : IPassengerSettings
    {
        public string PassengerCollectionName { get ; set ; }        
        public string RestrictPassengerCollectionName { get ; set ; }
        public string InactivePassengerCollectionName { get; set; }
        public string ConnectionString { get ; set ; }
        public string DatabaseName { get ; set ; }
    }
}
