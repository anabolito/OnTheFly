using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Newtonsoft.Json;

namespace Services
{
    public class AircraftService
    {
        static readonly HttpClient aircraftClient = new HttpClient();
        HttpResponseMessage responseMessage = new HttpResponseMessage();

        // Listar aircrafts
        public async Task<List<Aircraft>> Get()
        {
            try
            {
                HttpResponseMessage response = await AircraftService.aircraftClient.GetAsync("https://localhost:7036/api/AircraftAPI");
                response.EnsureSuccessStatusCode();
                string aircraft = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Aircraft>>(aircraft);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }
    }
}
