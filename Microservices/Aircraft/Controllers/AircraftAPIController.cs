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
        public ActionResult<Aircraft> Create(Aircraft aircraft)
        {
            return _aircraftAPIService.Create(aircraft);
        }

        [HttpPut("{id}")]
        public ActionResult Update(string id, Aircraft aircraft)
        {
            var c = _aircraftAPIService.Get(id);
            if (c == null) return NotFound();
            _aircraftAPIService.Update(aircraft);
            return Ok();
        }


        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            if (id == null) return NotFound();

            var aircraft = _aircraftAPIService.Get(id);
            if (aircraft == null) return NotFound();

            _aircraftAPIService.Delete(id);
            return Ok();
        }
    }
}
