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

        [HttpGet("{id}")]
        public ActionResult<Aircraft> Get(string id) => _aircraftAPIService.Get(id);

        [HttpPost]
        public ActionResult<Aircraft> Create(Aircraft aircraft) => _aircraftAPIService.Create(aircraft);

        [HttpPut("{id}")]
        public ActionResult Update(string id, Aircraft aircraft) => _aircraftAPIService.Update(id, aircraft);
  
        [HttpDelete("{id}")]
        public ActionResult Delete(string id) => _aircraftAPIService.Delete(id);

    }
}
