using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.DTOs;
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

        public async Task<List<Flight>> Get(string date)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + date);
                response.EnsureSuccessStatusCode();
                string flight = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Flight>>(flight);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public async Task<Flight> Post(FlightDTO flightDTO)
        {
            var destiny = new AirportService().GetIata(flightDTO.IataDestiny).Result;
            var departure = new AirportService().GetIata(flightDTO.IataDparture).Result;
            var plane = new AircraftService().GetById(flightDTO.RabPlane).Result;

            Flight flight = new Flight()
            {
                Destiny = destiny,
                Departure = departure,
                Plane = plane,
                DtDeparture = flightDTO.DtDeparture,
                Sales = flightDTO.Sales,
                Status = flightDTO.Status
            };

            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url, flight);
                response.EnsureSuccessStatusCode();
                string flightDeserialized = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Flight>(flightDeserialized);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public async Task<Flight> Put(string iata, string rab, string date)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url + "/" + iata + "/" + rab + "/" + date, new {IATA = iata, RAB = rab, Departure = date });
                response.EnsureSuccessStatusCode();
                string flightDeserialized = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Flight>(flightDeserialized);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public async Task<Flight> Delete(string iata, string rab, string date)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync(url + "/" + iata + "/" + rab + "/" + date);
                response.EnsureSuccessStatusCode();
                string flightDeserialized = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Flight>(flightDeserialized);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }
    }
}
