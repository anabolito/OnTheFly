﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Company
    {
        [BsonId]
        //[BsonElement("_id")]
        //[JsonProperty("_id")]
        [BsonRepresentation(BsonType.String)]
        public string CNPJ { get; set; }
        public string Name { get; set; }
        public string NameOpt { get; set; }
        public DateTime DtOpen { get; set; }
        public bool Status { get; set; }
        public Address Address { get; set; }
    }
}
