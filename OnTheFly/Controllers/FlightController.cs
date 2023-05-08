using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Services;

namespace OnTheFly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        #region Dependency Injection
        private readonly FlightService _flightService;

        public FlightController(FlightService flightService)
        {
            _flightService = flightService;
        }
        #endregion

        #region Get
        [HttpGet]
        public ActionResult<List<Flight>> GetFlight() => _flightService.Get().Result;

        [HttpGet("{departure}")]
        public ActionResult<List<Flight>> GetFlight(string departure) => _flightService.Get(departure).Result;
        #endregion

        #region Post
        [HttpPost]
        public ActionResult<Flight> PostFlight(FlightDTO flight) => _flightService.Post(flight).Result;
        #endregion

        #region Put
        [HttpPut("{iata}/{rab}/{date}")]
        public ActionResult<Flight> PutFlight(string iata, int rab, string date) =>
            _flightService.Put(iata, rab, date).Result;
        #endregion

        #region Delete
        [HttpDelete("{iata}/{rab}/{date}")]
        public ActionResult<Flight> DeleteFlight(string iata, int rab, string date) =>
            _flightService.Delete(iata, rab, date).Result;
        #endregion
    }
}
