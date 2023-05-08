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
        public ActionResult<List<Flight>> GetFlight() => _flightRepository.GetFlightAsync().Result;

        [HttpGet("{departure}")]
        public ActionResult<List<Flight>> GetFlight(string departure) => _flightRepository.GetFlightAsync(departure).Result;
        #endregion

        #region Post
        [HttpPost]
        public ActionResult<Flight> PostFlight(Flight flight) => _flightRepository.PostFlightAsync(flight).Result;
        #endregion
        
        #region Put
        [HttpPut("{iata}/{rab}/{date}")]
        public ActionResult<Flight> PutFlight(string iata, int rab, string date) => 
            _flightRepository.PutFlightAsync(iata, rab, date).Result;
        #endregion
        
        #region Delete
        [HttpDelete("{iata}/{rab}/{date}")]
        public ActionResult<Flight> DeleteFlight(string iata, int rab, string date) => 
            _flightRepository.DeleteFlightAsync(iata, rab, date).Result;
        #endregion
    }
}
