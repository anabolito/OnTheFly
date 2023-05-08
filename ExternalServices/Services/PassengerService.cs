using Models;
using Newtonsoft.Json;
using PassengerAPI.AddressService;
using PassengerAPI.DTO;
using System.Net.Http.Json;

namespace Services
{
    public class PassengerService
    {
        static readonly string url = "https://localhost:7264/api/Passenger/Passengers/";
        static readonly HttpClient client = new HttpClient();

        public async Task<List<Passenger>> Get()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Passenger>>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new (e.Message);
            }
        }

        public async Task<Passenger> Post(PassengerDTO passengerDTO, int number, string complement)
        {
            PostOffice post = new();
            var compPassenger = post.GetAddress(passengerDTO.CEP).Result;

            Address address = new()
            {
                Number = number,
                Complement = complement,

                Street = compPassenger.Street,
                Neighborhood = compPassenger.Neighborhood,
                City = compPassenger.City,
                State = compPassenger.State,
                ZipCode = compPassenger.ZipCode,
            };
            Passenger passenger = new()
            {
                CPF = passengerDTO.CPF,
                Name = passengerDTO.Name,
                Gender = passengerDTO.Gender,
                Phone = passengerDTO.Phone,
                DtBirth = passengerDTO.DtBirth,
                Status = passengerDTO.Status,
                Address = address,
            };

            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url, passenger);
                response.EnsureSuccessStatusCode();
                string passengerA = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passengerA);
            }
            catch (HttpRequestException ex)
            {
                throw new (ex.Message);
            }
        }

        public async Task<Passenger> Delete(string cpf)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync(url);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException ex)
            {
                throw new (ex.Message);
            }
        }
    }
}
