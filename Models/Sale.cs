using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Sale
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonIgnore]
        public string Id { get; set; }

        [BsonElement("Flight")]
        public Flight Flight{ get; set; }

        [BsonElement("Passengers")]
        public List<Passenger> Passengers { get; set; }
        
        [BsonElement("Reserved")]
        public bool Reserved { get; set; }
        
        [BsonElement("Sold")]
        public bool Sold { get; set; }


    }
}
