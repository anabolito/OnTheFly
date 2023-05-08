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
        [HttpPut]
        public ActionResult<Flight> PutFlight(Flight flight) => _flightRepository.PutFlightAsync(flight).Result;
        #endregion
        
        #region Delete
        [HttpDelete]
        public ActionResult<Flight> DeleteFlight(Flight flight) => _flightRepository.DeleteFlightAsync(flight).Result;
        #endregion
    }
}
