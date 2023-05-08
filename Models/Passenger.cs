using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PassengerAPI.DTO;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Passenger
    {
        #region[Properties]        
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string CPF { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Gender")]
        [BsonRepresentation(BsonType.String)]
        public char Gender { get; set; }

        [BsonElement("Phone")]
        public string? Phone { get; set; }

        [BsonElement("DtBirth")]
        public DateTime DtBirth { get; set; }

        [BsonElement("DtRegistry")]
        public DateTime DtRegistry { get; set; }

        [BsonElement("Status")]
        public bool? Status { get; set; }

        [BsonElement("Address")]
        public Address Address { get; set; }

        public Passenger()
        {
        }

        public Passenger(PassengerDTO dto, Address address)
        {
            CPF = dto.CPF;
            Name = dto.Name;
            Gender = dto.Gender;
            Phone = dto.Phone;
            DtBirth = dto.DtBirth;
            DtRegistry = dto.DtRegistry;
            Status = dto.Status;
            Address = address;
        }

        #endregion
    }
}
