using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace Models
{
    public class Flight
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Destiny")]
        public Airport Destiny { get; set; }

        [BsonElement("Departure")]
        public Airport Departure{ get; set; }

        [BsonElement("Plane")]
        public Aircraft Plane { get; set; }

        [BsonElement("Sales")]
        public int Sales { get; set; }

        [BsonElement("DtDeparture")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime DtDeparture { get; set; }

        [BsonElement("Status")]
        public bool Status { get; set; }
    }
}
