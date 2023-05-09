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

        [HttpGet]
        public ActionResult<List<Aircraft>> Get() => _aircraftAPIService.Get();

        [HttpGet("{id}", Name = "GetCapacity")]
        public int GetCapacity(string id) => _aircraftAPIService.GetCapacity(id);


        [HttpGet("{id}")]
        public ActionResult<Aircraft> Get(string id) => _aircraftAPIService.Get(id);

        [HttpPost]
        public ActionResult<Aircraft> Create(Aircraft aircraft) => _aircraftAPIService.Create(aircraft).Result;

        [HttpPut("UpdateDtLastFlight")]
        public ActionResult UpdateDtLastFlight(string id, Aircraft aircraft) => _aircraftAPIService.UpdateDtLastFlight(id, aircraft);

        [HttpPut("UpdateCompany")]
        public ActionResult UpdateCompany(string id, Aircraft aircraft) => _aircraftAPIService.UpdateCompany(id, aircraft).Result;

        [HttpDelete("{id}")]
        public ActionResult Delete(string id) => _aircraftAPIService.Delete(id);

    }
}
