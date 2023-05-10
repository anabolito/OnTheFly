using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Airport
    {
        public string IATA { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
