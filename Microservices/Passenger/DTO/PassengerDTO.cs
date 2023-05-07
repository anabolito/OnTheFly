using Models;
using MongoDB.Bson.Serialization.Attributes;

namespace PassengerAPI.DTO
{
    public class PassengerDTO
    {
        public string Name { get; set; }       
        public char Gender { get; set; }        
        public string? Phone { get; set; }        
        public DateOnly DtBirth { get; set; }
        public DateTime DtRegistry { get; set; }        
        public bool? Status { get; set; }        
        public Address Address { get; set; } 
    }
}
