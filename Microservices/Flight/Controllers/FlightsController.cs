using FlightAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models;

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

        #region Get
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
        #endregion

        #region Post
        [HttpPost]
        public ActionResult<Flight> PostFlight(Flight flight)
        {
            flight.Id = null;
            if (_flightRepository.PostFlightAsync(flight).Result != null)
                return Ok(flight);
            else
                return BadRequest();
        }
        #endregion

        #region Put
        [HttpPut("{iata}/{rab}/{date}")]
        public ActionResult<Flight> PutFlight(string iata, string rab, string date)
        {
            if (_flightRepository.GetFlightAsync(iata, rab, date).Result == null)
                return NotFound();

            var flight = _flightRepository.PutFlightAsync(iata, rab, date).Result;

            if (flight != null)
                return Ok(flight);
            else
                return BadRequest();
        }
        #endregion

        #region Delete
        [HttpDelete("{iata}/{rab}/{date}")]
        public ActionResult DeleteFlight(string iata, string rab, string date)
        {
            if (_flightRepository.DeleteFlightAsync(iata, rab, date).Result)
                return Accepted();
            else
                return NotFound();
        }
        #endregion
    }
}
