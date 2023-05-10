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
using System.Net;

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

        [HttpGet("CPF")]
        public async Task<Passenger> GetByCPF(string cpf)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + cpf);
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

        [HttpPut("/{Address}/CPF")]
        public async Task<Passenger> UpdatePassengerAddress(string cpf, int number, string? complement, string cep)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url + cpf + "/Address", new { cpf, number, complement, cep });
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }

        [HttpPut("/Street/CPF")]
        public async Task<Passenger> UpdatePassengerAddressStreet(string cpf, string streetName)   {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url + "/Street",streetName);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }

        [HttpPut("/{Name}/CPF")]
        public async Task<Passenger> UpdatePassengerName(string cpf, string name)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url + cpf + "/Name", name);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }

        [HttpPut("/Gen/CPF")]
        public async Task<Passenger> UpdatePassengerGender(string cpf, char gen)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url + cpf + "/Gen", gen);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }

        [HttpPut("/Phone/CPF")]
        public async Task<Passenger> UpdatePassengerPhone(string cpf, string phone)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url + cpf + "/Phone", phone);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }       

        [HttpPut("/Status/CPF")]
        public async Task<Passenger> UpdatePassengerStatus(string cpf, bool stat)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url + cpf + "/Status", stat);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }

        [HttpPut("/SetRestrict/CPF")]
        public async Task<Passenger> SetPassengerAsRestricted(string cpf)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url, cpf);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }

        [HttpPut("/SetUnrestrict/CPF")]
        public async Task<Passenger> SetPassengerAsUnRestricted(string cpf)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url, cpf);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }

        [HttpPut("/Reactivate/CPF")]
        public async Task<Passenger> ReactivatePassenger(string cpf)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url + "/Reactivate", cpf);
                response.EnsureSuccessStatusCode();
                string passenger = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Passenger>(passenger);
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }
                
        [HttpDelete("/Desactivate/CPF")]
        public async Task<Passenger> Delete(string cpf)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url + "/Delete", cpf);
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
