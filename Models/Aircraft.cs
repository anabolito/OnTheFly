using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Aircraft
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string RAB { get; set; }

        [BsonElement("Capacity")]
        public int Capacity { get; set; }

        [BsonElement("DtLastFlight")]
        public DateTime? DtLastFlight { get; set; }

        [BsonElement("DtRegistry")]
        public DateTime DtRegistry { get; set; }

        [BsonElement("Company")]
        public Company Company { get; set; }
    }
}
