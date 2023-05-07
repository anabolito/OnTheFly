namespace PassengerAPI.Utils
{
    public interface IPassengerSettings
    {
        public string PassengerCollectionName { get; set; }     
        public string UnderagePassengerCollectionName { get; set; }     
        public string RestrictPassengerCollectionName { get; set; }     
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }        
    }
}
