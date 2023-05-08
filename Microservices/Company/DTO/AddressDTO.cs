using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CompanyAPI.DTO
{
    public class AddressDTO
    {
        public int Id { get; set; }


        [JsonProperty("pais")]
        public string? Country { get; set; }


        [JsonProperty("cep")]
        public string ZipCode { get; set; }


        [JsonProperty("bairro")]
        public string Neighborhood { get; set; }


        [JsonProperty("localidade")]
        public string City { get; set; }


        [JsonProperty("uf")]
        public string State { get; set; }


        [JsonProperty("logradouro")]
        public string Street { get; set; }


        //[JsonProperty("gia")]
        //public string Number { get; set; }


        [JsonProperty("complemento")]
        public string Complety { get; set; }

        //public AddressDTO(int id, string? street, string? complety, string? neighborhood, string? city, string? state, string? zipCode, string? number)
        //{
        //    Id = id;
        //    Street = street;
        //    Complety = complety;
        //    Neighborhood = neighborhood;
        //    City = city;
        //    State = state;
        //    ZipCode = zipCode;
        //    Number = number;
        //}
    }
}
