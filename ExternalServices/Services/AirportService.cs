using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportAPI.Models;
using Models;
using Newtonsoft.Json;

namespace Services
{
    public class AirportService
    {
        private readonly string url = "https://localhost:44366/Airport/";
        static readonly HttpClient client = new HttpClient();

        public async Task<AirportPestanic> GetIata(string iata)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + iata);
                response.EnsureSuccessStatusCode();
                string flight = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<AirportPestanic>>(flight).FirstOrDefault();
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public async Task<List<AirportPestanic>> GetState(string state)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + $"/ByState/{state}");
                response.EnsureSuccessStatusCode();
                string flight = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<AirportPestanic>>(flight);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public async Task<List<AirportPestanic>> GetCityCode(string cityCode)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + $"/ByCity/{cityCode}");
                response.EnsureSuccessStatusCode();
                string flight = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<AirportPestanic>>(flight);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public async Task<List<AirportPestanic>> GetCity(string city)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + $"/ByCityName/{city}");
                response.EnsureSuccessStatusCode();
                string flight = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<AirportPestanic>>(flight);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public async Task<List<AirportPestanic>> GetIcao(string icao)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + $"/ByIcao/{icao}");
                response.EnsureSuccessStatusCode();
                string flight = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<AirportPestanic>>(flight);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }
    }
}
