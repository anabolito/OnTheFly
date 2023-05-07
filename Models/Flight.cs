using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace Models
{
    public class Flight
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string Id { get; set; }

        [BsonElement("Destiny")]
        public Airport Destiny { get; set; }

        [JsonPropertyName("Departure")]
        public Airport Departure{ get; set; }

        [JsonPropertyName("Plane")]
        public Aircraft Plane { get; set; }

        [JsonPropertyName("Sales")]
        public int Sales { get; set; }

        [JsonPropertyName("DtDeparture")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime DtDeparture { get; set; }

        [JsonPropertyName("Status")]
        [BsonRepresentation(BsonType.Boolean)]
        public bool Status { get; set; }
    }
}
