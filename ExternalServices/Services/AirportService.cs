using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Newtonsoft.Json;

namespace Services
{
    public class AirportService
    {
        private readonly string url = "https://localhost:44366/Airport/";
        static readonly HttpClient client = new HttpClient();

        public async Task<Airport> GetIata(string iata)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + iata);
                response.EnsureSuccessStatusCode();
                string flight = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Airport>>(flight).FirstOrDefault();
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public async Task<List<Airport>> GetState(string state)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + $"/ByState/{state}");
                response.EnsureSuccessStatusCode();
                string flight = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Airport>>(flight);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public async Task<List<Airport>> GetCityCode(string cityCode)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + $"/ByCity/{cityCode}");
                response.EnsureSuccessStatusCode();
                string flight = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Airport>>(flight);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public async Task<List<Airport>> GetCity(string city)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + $"/ByCityName/{city}");
                response.EnsureSuccessStatusCode();
                string flight = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Airport>>(flight);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public async Task<List<Airport>> GetIcao(string icao)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + $"/ByIcao/{icao}");
                response.EnsureSuccessStatusCode();
                string flight = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Airport>>(flight);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }
    }
}
