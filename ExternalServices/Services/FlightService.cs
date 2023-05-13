using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Models;
using Models.DTOs;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;

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

        public async Task<Flight> Get(string iata, string rab, string date)
        {
            string teste = HttpUtility.UrlEncode(date);
            //      10%2F05%2F2023
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + iata + "/" + rab + "/" + teste);
                response.EnsureSuccessStatusCode();
                string flight = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Flight>(flight);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public async Task<Flight> Post(FlightDTO flightDTO)
        {
            var destinyPestanic = new AirportService().GetIata(flightDTO.IataDestiny).Result;
            var plane = new AircraftService().GetById(flightDTO.RabPlane).Result;

            Airport destiny = new Airport()
            {
                IATA = destinyPestanic.iata,
                City = destinyPestanic.city,
                State = destinyPestanic.state,
                Country = destinyPestanic.country_id
            };

            if ((destiny == null) && (plane == null))
                return null;

            Flight flight = new Flight()
            {
                Arrival = destiny,
                Plane = plane,
                DtDeparture = flightDTO.DtDeparture,
                Sales = 0,
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
            date = HttpUtility.UrlEncode(date);
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

        public async Task<Flight> PutSalesCountAsync(string iata, string rab, string date, int count)
        {
            date = HttpUtility.UrlEncode(date);
            var data = new { count };
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(url + "Count/" + iata + "/" + rab + "/" + date, data);
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
