using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Newtonsoft.Json;

namespace Services
{
    public class FlightService
    {
        static readonly string url = "https://localhost:7029/api/Flights/";
        static readonly HttpClient client = new HttpClient();

        public async Task<List<Flight>> Get()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string flight = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Flight>>(flight);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }
    }
}
