using Models.DTOs;
using Newtonsoft.Json;

namespace Services
{
    public class AirportService
    {
        private readonly string url = "https://localhost:5001/Airport/";
        static readonly HttpClient client = new HttpClient();

        public async Task<AirportDTO> GetIata(string iata)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + iata);
                response.EnsureSuccessStatusCode();
                string flight = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AirportDTO>(flight);
            }
            catch (HttpRequestException e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                throw;
            }
        }

        public async Task<List<AirportDTO>> GetState(string state)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + $"/ByState/{state}");
                response.EnsureSuccessStatusCode();
                string flight = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<AirportDTO>>(flight);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public async Task<List<AirportDTO>> GetCityCode(string cityCode)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + $"/ByCity/{cityCode}");
                response.EnsureSuccessStatusCode();
                string flight = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<AirportDTO>>(flight);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public async Task<List<AirportDTO>> GetCity(string city)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + $"/ByCityName/{city}");
                response.EnsureSuccessStatusCode();
                string flight = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<AirportDTO>>(flight);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public async Task<List<AirportDTO>> GetIcao(string icao)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + $"/ByIcao/{icao}");
                response.EnsureSuccessStatusCode();
                string flight = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<AirportDTO>>(flight);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }
    }
}
