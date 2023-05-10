using Models;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace PassengerAPI.DTO
{
    public class PassengerDTO
    {       
        public string CPF { get; set; }
        public string Name { get; set; }       
        public char Gender { get; set; }        
        public string? Phone { get; set; }        
        public DateTime DtBirth { get; set; }               
        public bool? Status { get; set; }        
        public string CEP { get; set; } 
        public int Number { get; set; }
        public string Complement { get; set; }        
    }
}
