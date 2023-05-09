using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using PassengerAPI.DTO;
using Services;

namespace OnTheFly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly PassengerService _passengerService;        

        public PassengerController(PassengerService passengerService)
        {
            _passengerService = passengerService;           
        }
        
        [HttpGet("Passengers")]
        public ActionResult<List<Passenger>> Get()
        {
            return Ok(_passengerService.Get());

        }

        [HttpGet("UndeAgeOnes")]
        public ActionResult<List<Passenger>> GetAllMinors()
        {
            var minors = _passengerService.GetAllMinors();
            return Ok(minors);
        }


        [HttpGet("Inactives")]
        public ActionResult<List<Passenger>> GetDeletedOnes()
        {
            return Ok(_passengerService.GetDeletedOnes());
        }

        [HttpGet("{_id}")]
        public ActionResult<Passenger> Get(string _id)
        {
            var passenger = _passengerService.GetByCPF(_id);
            if (passenger == null) return new Passenger();
            return Ok(passenger);
        }

        [HttpPost]
        public ActionResult Post(PassengerDTO passengeDTO, int number, string complement)
        {

            Address address = new()
            {
                Number = number,
                Complement = complement,
                ZipCode = passengeDTO.CEP,
            };

            Passenger passenger = new(passengeDTO, address);

            try
            {               
                return StatusCode(201);

            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}/Address")]
        public ActionResult<Passenger> UpdatePassengerAddress(string id, int number, string? complement, string cep)
        {
            var passenger = _passengerService.UpdatePassengerAddress(id, number, complement, cep);
            if (passenger == null) return NotFound();

            return Ok(passenger);
        }

        [HttpPut("{id}/Name")]
        public ActionResult<Passenger> UpdatePassengerName(string id, string name)
        {
            var passenger = _passengerService.UpdatePassengerName(id, name);
            if (passenger == null) return NotFound();

            return Ok(passenger);
        }

        [HttpPut("{id}/Gen")]
        public ActionResult<Passenger> UpdatePassengerGender(string id, char gen)
        {
            var passenger = _passengerService.UpdatePassengerGender(id, gen);
            if (passenger == null) return NotFound();

            return Ok(passenger);
        }

        [HttpPut("{id}/Phone")]
        public ActionResult<Passenger> UpdatePassengerPhone(string id, string phone)
        {
            var passenger = _passengerService.UpdatePassengerPhone(id, phone);
            if (passenger == null) return NotFound();

            return Ok(passenger);
        }

        [HttpPut("{id}/BirthDate")]
        public ActionResult<Passenger> UpdatePassengerBirthDate(string id, string birth)
        {

            var passenger = _passengerService.UpdatePassengerBirthDate(id, birth);
            if (passenger == null) return NotFound();

            return Ok(passenger);
        }

        [HttpPut("{id}/RegisterDate")]
        public ActionResult<Passenger> UpdatePassengerRegisterDate(string id, string reg)
        {
            var passenger = _passengerService.UpdatePassengerRegisterDate(id, reg);
            if (passenger == null) return NotFound();

            return Ok(passenger);
        }

        [HttpPut("{id}/Status")]
        public ActionResult<Passenger> UpdatePassengerStatus(string id, bool stat)
        {
            var passenger = _passengerService.UpdatePassengerStatus(id, stat);
            if (passenger == null) return NotFound();

            return Ok(passenger);
        }

        [HttpPut("{id}/Reactivate")]
        public void ReactivatePassenger(string id)
        {
            _passengerService.ReactivatePassenger(id);
        }
        
        [HttpDelete("{id}/Delete")]
        public void Delete(string id)
        {
            _passengerService.Delete(id);
        }
    }
}
