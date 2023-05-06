using AirportAPI.Models;
using AirportAPI.Serivces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AirportAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly AirportService _airportServices;
        public AirportController(AirportService airportServices)
        {
            _airportServices = airportServices;
        }

        [HttpGet("{iata}", Name = "GetAirportIata")]
        public ActionResult<Airport> Get(string iata)
        {
            var airport = _airportServices.Get(iata);

            if (airport == null)
                return NotFound();

            return airport;
        }
        
        [HttpGet("/ByState/{state}", Name = "GetAirportState")]
        public ActionResult<List<Airport>> GetByState(string state)
        {
            var airport = _airportServices.GetByState(state);

            if (airport.Count == 0)
                return NotFound();

            return airport;
        }
        [HttpGet("/ByCity/{city_code}", Name = "GetAirportCityCode")]
        public ActionResult<List<Airport>> GetByCityCode(string city_code)
        {
            var airport = _airportServices.GetByCityCode(city_code);

            if (airport.Count == 0)
                return NotFound();

            return airport;
        }

        [HttpGet("/ByCityName/{city}", Name = "GetAirportCityName")]
        public ActionResult<List<Airport>> GetByCityName(string city)
        {
            var airport = _airportServices.GetByCityName(city);

            if (airport.Count == 0)
                return NotFound();

            return airport;
        }

        [HttpGet("/ByIcao/{icao}", Name = "GetAirportIcao")]
        public ActionResult<Airport> GetByIcao(string icao)
        {
            var airport = _airportServices.GetByIcao(icao);

            if (airport == null)
                return NotFound();

            return airport;
        }
    }
}
