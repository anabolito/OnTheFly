using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using PassengerAPI.AddressService;
using PassengerAPI.DTO;
using PassengerAPI.Service;
using System.Net.Http.Json;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;

namespace Services
{
    public class PassengerService
    {
        static readonly string url = "https://localhost:7264/api/Passenger/Passengers/";
        static readonly HttpClient client = new HttpClient();

        [HttpGet("GetAllActives")]
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
                throw new(e.Message);
            }
        }
        [HttpGet("UnderAgeOnes")]
        public async Task<List<Passenger>> GetAllMinors()
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
                throw new(e.Message);
            }
        }

        [HttpGet("Inactives")]
        public async Task<List<Passenger>> GetDeletedOnes()
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
                throw new(e.Message);
            }
        }        

        [HttpGet("RestrictedOnes")]
        public async Task<List<Passenger>> GetRestrictedOnes()
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
                throw new(e.Message);
            }
        }

        [HttpGet("{_id}")]
        public async Task<Passenger> GetByCPF(string _id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + _id);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }

        [HttpPost]
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
                throw new(ex.Message);
            }
        }

        [HttpPut("{id}/{Address}")]
        public async Task<Passenger> UpdatePassengerAddress(string id, int number, string? complement, string cep)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url + id + "/Address", new { id, number, complement, cep });
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }

        [HttpPut("{id}/{Name}")]
        public async Task<Passenger> UpdatePassengerName(string id, string name)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url + id + "/Name", name);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }

        [HttpPut("{id}/Gen")]
        public async Task<Passenger> UpdatePassengerGender(string id, char gen)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url + id + "/Gen", gen);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }

        [HttpPut("{id}/Phone")]
        public async Task<Passenger> UpdatePassengerPhone(string id, string phone)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url + id + "/Phone", phone);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }

        [HttpPut("{id}/BirthDate")]
        public async Task<Passenger> UpdatePassengerBirthDate(string id, string birth)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url + id + "/BirthDate", birth);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }

        }

        [HttpPut("{id}/RegisterDate")]
        public async Task<Passenger> UpdatePassengerRegisterDate(string id, string reg)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url + id + "/RegisterDate", reg);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }

        [HttpPut("{id}/Status")]
        public async Task<Passenger> UpdatePassengerStatus(string id, bool stat)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url + id + "/Status", stat);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }

        [HttpPut("{id}/SetRestrict")]
        public async Task<Passenger> SetPassengerAsRestricted(string id)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url, id);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }

        [HttpPut("{id}/SetUnrestrict")]
        public async Task<Passenger> SetPassengerAsUnRestricted(string id)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url, id);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }

        [HttpPut("{id}/Reactivate")]
        public async Task<Passenger> ReactivatePassenger(string id)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url + "/Reactivate", id);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }
                
        [HttpDelete("{id}/Desactivate")]
        public async Task<Passenger> Delete(string id)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url + "/Delete", id);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }
    }  
}
