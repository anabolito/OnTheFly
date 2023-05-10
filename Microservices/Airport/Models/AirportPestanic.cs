using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AirportAPI.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AirportPestanic
    {
        //[BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("iata")]
        [BsonRepresentation(BsonType.String)]
        public string iata { get; set; }
        [BsonElement("time_zone_id")]
        public string time_zone_id { get; set; }
        [BsonElement("name")]
        public string name { get; set; }
        [BsonElement("city_code")]
        public string city_code { get; set; }
        [BsonElement("country_id")]
        public string? country_id { get; set; }
        [BsonElement("location")]
        public string location { get; set; }
        [BsonElement("elevation")]
        public string elevation { get; set; }
        [BsonElement("icao")]
        public string icao { get; set; }
        [BsonElement("city")]
        public string city { get; set; }
        [BsonElement("state")]
        public string? state { get; set; }
    }
}
