using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CompanyAPI.DTO
{
    public class AddressDTO
    {
        public int Id { get; set; }


        [JsonPropertyName("pais")]
        public string? Country { get; set; }


        [JsonPropertyName("cep")]
        public string ZipCode { get; set; }


        [JsonPropertyName("bairro")]
        public string Neighborhood { get; set; }


        [JsonPropertyName("localidade")]
        public string City { get; set; }


        [JsonPropertyName("uf")]
        public string State { get; set; }


        [JsonPropertyName("logradouro")]
        public string Street { get; set; }


        [JsonPropertyName("gia")]
        public string Number { get; set; }


        [JsonPropertyName("complemento")]
        public string Complety { get; set; }

        public AddressDTO(int id, string? street, string? complety, string? neighborhood, string? city, string? state, string? zipCode, string? number)
        {
            Id = id;
            Street = street;
            Complety = complety;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            ZipCode = zipCode;
            Number = number;
        }
    }
}
