using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Models.DTOs
{
    public class AirportDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("iata")]
        [BsonRepresentation(BsonType.String)]
        public string iata { get; set; }

        [BsonElement("state")]
        public string? state { get; set; }

        [BsonElement("city")]
        public string city { get; set; }

        [BsonElement("country_id")]
        public string? country_id { get; set; }
    }
}
