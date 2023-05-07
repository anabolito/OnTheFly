using PassengerAPI.DTO;

namespace Models
{
    public class Address
    {
        public string ZipCode { get; set; }
        public string? Street { get; set; }
        public int Number { get; set; }
        public string? Complement { get; set; }
        public string? Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        #region[Ctors]
        public Address()
        {
        }

        public Address(AddressDTO addressDTO)
        {
            Street = addressDTO.Street;            
            Neighborhood = addressDTO.Neighborhood;
            City = addressDTO.City;
            Complement = addressDTO.Complement;
            State = addressDTO.State;
        }
        #endregion
    }
}
