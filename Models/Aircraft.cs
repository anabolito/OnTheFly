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
        public int Capacity { get; set; }
        public DateTime DtLastFlight { get; set; }
        public DateTime DtRegistry { get; set; }
        public Company Company { get; set; }
    }
}
