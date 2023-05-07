using Newtonsoft.Json;

namespace PassengerAPI.DTO
{
    public class AddressDTO
    {
        public int Id { get; set; }

        [JsonProperty("logradouro")]
        public string? Street { get; set; }

        [JsonProperty("complemento")]
        public string? Complement { get; set; }

        [JsonProperty("bairro")]
        public string? Neighborhood { get; set; }

        [JsonProperty("localidade")]
        public string? City { get; set; }

        [JsonProperty("uf")]
        public string? State { get; set; }

        [JsonProperty("cep")]
        public string? ZipCode { get; set; }

        public AddressDTO(int id, string? street, string? complement, string? neighborhood, string? city, string? state, string? zipCode)
        {
            Id = id;
            Street = street;
            Complement = complement;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            ZipCode = zipCode;
        }
    }
}
