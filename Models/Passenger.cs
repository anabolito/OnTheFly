using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Passenger
    {
        #region[Properties]

        [BsonId]
        [BsonElement("CPF")]
        [RegularExpression(@"^\d{3}.\d{3}.\d{3}-\d{2}$", ErrorMessage = "Formato do CPF inválido")]
        
        public string CPF { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Gender")]
        public char Gender { get; set; }

        [BsonElement("Phone")]
        public string? Phone{ get; set; }

        [BsonElement("DtBirth")]
        public DateTime DtBirth { get; set; }

        [BsonElement("DtRegistry")]
        public DateTime DtRegistry { get; set; }

        [BsonElement("Status")]
        public bool? Status { get; set; }

        [BsonElement("Address")]
        public Address Address{ get; set; }

        #endregion
    }
}
