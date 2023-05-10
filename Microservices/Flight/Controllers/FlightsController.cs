using FlightAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;

namespace FlightAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        #region Dependency Injection
        private readonly FlightRepository _flightRepository;

        public FlightsController(FlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }
        #endregion

        
        [HttpGet]
        public ActionResult<List<Flight>> GetFlight()
        {
            List<Flight> list = _flightRepository.GetFlightAsync().Result;
            if (list != null)
                return Ok(list);
            else
                return NoContent();
        }

        [HttpGet("{departure}")]
        public ActionResult<List<Flight>> GetFlight(string departure)
        {
            List<Flight> list = _flightRepository.GetFlightAsync(departure).Result;
            if (list != null)
                return Ok(list);
            else
                return NoContent();
        }

        [HttpGet("{iata}/{rab}/{date}")]
        public ActionResult<Flight> GetFlight(string iata, string rab, string date)
        {
            Flight flight = _flightRepository.GetFlightAsync(iata, rab, date).Result;
            if (flight != null)
                return Ok(flight);
            else
                return NoContent();
        }
        
        [HttpPost]
        public ActionResult<Flight> PostFlight(FlightDTO flight)
        {
            if (_flightRepository.PostFlightAsync(flight).Result != null)
                return StatusCode(201, flight);
            else
                return BadRequest();
        }
        
        [HttpPut("{iata}/{rab}/{date}")]
        public ActionResult<Flight> PutFlight(string iata, string rab, string date)
        {
            if (_flightRepository.GetFlightAsync(iata, rab, date).Result == null)
                return NotFound();

            var flight = _flightRepository.PutFlightAsync(iata, rab, date).Result;

            if (flight != null)
                return StatusCode(201, flight);
            else
                return BadRequest();
        }

        [HttpDelete("{iata}/{rab}/{date}")]
        public ActionResult DeleteFlight(string iata, string rab, string date)
        {
            if (_flightRepository.DeleteFlightAsync(iata, rab, date).Result)
                return Ok();
            else
                return NotFound();
        }

    }
}
