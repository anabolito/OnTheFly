using AirportAPI.Models;
using AirportAPI.Utils;
using MongoDB.Driver;
using System.Collections.Generic;

namespace AirportAPI.Serivces
{
    public class AirportService
    {
        private readonly IMongoCollection<AirportPestanic> _airports;

        public AirportService(IDataBaseSettings settings)
        {
            var airport = new MongoClient(settings.ConnectionString);
            var database = airport.GetDatabase(settings.DataBaseName);
            _airports = database.GetCollection<AirportPestanic>(settings.AirportCollectionName);
        }

        public List<AirportPestanic> Get() =>
            _airports.Find(airport => true).ToList();

        public AirportPestanic Get(string iata) =>
            _airports.Find<AirportPestanic>(airport => airport.iata == iata).FirstOrDefault();

        public AirportPestanic GetByIcao(string icao) =>
            _airports.Find<AirportPestanic>(airport => airport.icao == icao).FirstOrDefault();

        public List<AirportPestanic> GetByState(string state) =>
            _airports.Find<AirportPestanic>(airport => airport.state == state).ToList();
        public List<AirportPestanic> GetByCityCode(string city_code) =>
            _airports.Find<AirportPestanic>(airport => airport.city_code == city_code).ToList();

        public List<AirportPestanic> GetByCityName(string city) =>
            _airports.Find<AirportPestanic>(airport => airport.city == city).ToList();

        public List<AirportPestanic> GetByCountry(string country_id) =>
            _airports.Find<AirportPestanic>(airport => airport.country_id == country_id).ToList();
            
    }
}