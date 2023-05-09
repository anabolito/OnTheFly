using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
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
        //[HttpGet]
        //public ActionResult<List<Flight>> GetFlight()
        //{
        //    List<Flight> list = _flightService.Get().Result;
        //    if (list != null)
        //        return Ok(list);
        //    else
        //        return NoContent();
        //}

        //[HttpGet("{departure}")]
        //public ActionResult<List<Flight>> GetFlight(string departure)
        //{
        //    List<Flight> list = _flightService.Get(departure).Result;
        //    if (list != null)
        //        return Ok(list);
        //    else
        //        return NoContent();
        //}

        //[HttpGet("{iata}/{rab}/{date}")]
        //public ActionResult<List<Flight>> GetFlight(string iata, string rab, string date)
        //{
        //    Flight flight = _flightService.Get(iata, rab, date).Result;
        //    if (flight != null)
        //        return Ok(flight);
        //    else
        //        return NoContent();
        //}
        #endregion

        #region Post
        //[HttpPost("Flight", Name = "PostFlight")]
        //public ActionResult<Flight> PostFlight(FlightDTO flightDTO)
        //{
        //    Flight flight = _flightService.Post(flightDTO).Result;
        //    if (flight == null)
        //        return BadRequest();
        //    else
        //        return Ok();
        //}
        #endregion

        #region Put
        //[HttpPut("{iata}/{rab}/{date}", Name = "PutFlight")]
        //public ActionResult<Flight> PutFlight(string iata, string rab, string date) =>
        //_flightService.Put(iata, rab, date).Result;
        #endregion

        #region Delete
        [HttpDelete("{iata}/{rab}/{date}", Name = "DeleteFlight")]
        public ActionResult<Flight> DeleteFlight(string iata, string rab, string date) =>
            _flightService.Delete(iata, rab, date).Result;
        #endregion
    }
}
