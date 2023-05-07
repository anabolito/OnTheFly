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
        public ActionResult<List<Flight>> GetFlight() => _flightRepository.GetFlight();

        [HttpGet("{Date}")]
        public ActionResult<List<Flight>> GetFlight(DateOnly departure) => _flightRepository.GetFlight(departure);
        #endregion

        #region Post
        [HttpPost]
        public ActionResult<Flight> PostFlight(Flight flight) => _flightRepository.PostFlight(flight);
        #endregion
        
        #region Put
        [HttpPut]
        public ActionResult<Flight> PutFlight(Flight flight) => _flightRepository.PutFlight(flight);
        #endregion
        
        #region Delete
        [HttpDelete]
        public ActionResult<Flight> DeleteFlight(Flight flight) => _flightRepository.DeleteFlight(flight);
        #endregion
    }
}
