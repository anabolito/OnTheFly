using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Airport
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string IATA { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
