using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
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

        // Buscar aeronave por ID
        public async Task<Aircraft> GetById(string rab)
        {
            try
            {
                HttpResponseMessage response = await AircraftService.aircraftClient.GetAsync("https://localhost:7036/api/AircraftAPI" + $"/{rab}");
                response.EnsureSuccessStatusCode();
                string aircraft = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Aircraft>(aircraft);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        // Retornar o numero de acentos da aeronave
        public async Task<Aircraft> GetCapacity(string rab)
        {
            try
            {
                HttpResponseMessage response = await AircraftService.aircraftClient.GetAsync("https://localhost:7036/api/AircraftAPI" + $"/{rab}");
                response.EnsureSuccessStatusCode();
                string aircraft = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Aircraft>(aircraft);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }


        // Inserir aeronave
        public async Task<Aircraft> Insert(Aircraft c)
        {
            try
            {
                HttpResponseMessage response = await AircraftService.aircraftClient.PostAsJsonAsync("https://localhost:7036/api/AircraftAPI", c);
                response.EnsureSuccessStatusCode();
                string aircraft = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Aircraft>(aircraft);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        // Atualiza
        public async Task<Aircraft> Update(Aircraft c)
        {
            try
            {
                HttpResponseMessage response = await AircraftService.aircraftClient.PutAsJsonAsync("https://localhost:7036/api/AircraftAPI" + $"/{c.RAB}", c);
                response.EnsureSuccessStatusCode();
                string aircraft = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Aircraft>(aircraft);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        // Deleta
        public async Task<Aircraft> Delete(string rab)
        {
            try
            {
                HttpResponseMessage response = await AircraftService.aircraftClient.DeleteAsync("https://localhost:7036/api/AircraftAPI" + $"/{rab}");
                response.EnsureSuccessStatusCode();
                string aircraft = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Aircraft>(aircraft);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

    }
}
