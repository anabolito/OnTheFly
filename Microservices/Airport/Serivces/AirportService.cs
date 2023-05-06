using AirportAPI.Models;
using AirportAPI.Utils;
using MongoDB.Driver;
using System.Collections.Generic;

namespace AirportAPI.Serivces
{
    public class AirportService
    {
        private readonly IMongoCollection<Airport> _airports;

        public AirportService(IDataBaseSettings settings)
        {
            var airport = new MongoClient(settings.ConnectionString);
            var database = airport.GetDatabase(settings.DataBaseName);
            _airports = database.GetCollection<Airport>(settings.AirportCollectionName);
        }

        public List<Airport> Get() =>
            _airports.Find(airport => true).ToList();

        public Airport Get(string iata) =>
            _airports.Find<Airport>(airport => airport.iata == iata).FirstOrDefault();

        public Airport GetByIcao(string icao) =>
            _airports.Find<Airport>(airport => airport.icao == icao).FirstOrDefault();

        public List<Airport> GetByState(string state) =>
            _airports.Find<Airport>(airport => airport.state == state).ToList();
        public List<Airport> GetByCityCode(string city_code) =>
            _airports.Find<Airport>(airport => airport.city_code == city_code).ToList();

        public List<Airport> GetByCityName(string city) =>
            _airports.Find<Airport>(airport => airport.city == city).ToList();

        public List<Airport> GetByCountry(string country_id) =>
            _airports.Find<Airport>(airport => airport.country_id == country_id).ToList();
            
    }
}