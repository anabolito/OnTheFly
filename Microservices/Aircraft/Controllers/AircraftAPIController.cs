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

        [HttpPost]
        public ActionResult<Aircraft> Create(Aircraft aircraft)
        {
            return _aircraftAPIService.Create(aircraft);
        }


    }
}
