using AircraftAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AircraftAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftAPIController : ControllerBase
    {
        protected readonly AircraftAPIRepository _aircraftAPIService;

        public AircraftAPIController(AircraftAPIRepository aircraftAPIService)
        {
            _aircraftAPIService = aircraftAPIService;
        }

        [HttpPost]
        public ActionResult<Aircraft> Create(Aircraft aircraft)
        {
            try
            {
                _aircraftAPIService.Create(aircraft);
                return StatusCode(201, aircraft);
            }
            catch (BadHttpRequestException ex)
            {
                return NotFound(ex.Message); ;
            }
        }

        [HttpGet]
        public ActionResult<List<Aircraft>> Get()
        {
            try
            {
                var list = _aircraftAPIService.Get();
                return StatusCode(201, list);
            }
            catch (BadHttpRequestException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("Capacity/{rab}", Name = "GetCapacity")]
        public ActionResult<int> GetCapacity(string rab)
        {
            try
            {
                return _aircraftAPIService.GetCapacity(rab);
            }
            catch (BadHttpRequestException ex)
            {
                return NotFound(ex.Message); ;
            }
        }


        [HttpGet("{rab}")]
        public ActionResult<Aircraft> Get(string rab)
        {
            try
            {
                var aircraft = _aircraftAPIService.Get(rab);
                return StatusCode(201, aircraft);
            }
            catch (BadHttpRequestException ex)
            {
                return NotFound(ex.Message); ;
            }
        }


        [HttpPut("UpdateDtLastFlight/{rab}")]
        public ActionResult UpdateDtLastFlight(string rab, Aircraft aircraft)
        {
            try
            {
                _aircraftAPIService.UpdateDtLastFlight(rab, aircraft);
                return StatusCode(201, aircraft);
            }
            catch (BadHttpRequestException ex)
            {
                return NotFound(ex.Message); ;
            }
        }


        [HttpPut("UpdateCompany/{rab}")]
        public ActionResult UpdateCompany(string rab, Aircraft aircraft)
        {
            try
            {
                _aircraftAPIService.UpdateCompany(rab, aircraft);
                return StatusCode(201, aircraft);
            }
            catch (BadHttpRequestException ex)
            {
                return NotFound(ex.Message); ;
            }
        } 

        [HttpDelete("{rab}")]
        public ActionResult Delete(string rab)
        {
            if (rab == null) return NotFound();
            _aircraftAPIService.Delete(rab);

            return StatusCode(201);

        } 

    }
}
