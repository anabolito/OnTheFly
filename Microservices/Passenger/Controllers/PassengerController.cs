using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Models;
using PassengerAPI.AddressService;
using PassengerAPI.DTO;
using PassengerAPI.Repositories;
using PassengerAPI.Service;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace PassengerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly PassengerRepository _passengerService;
        private readonly PostOffice _postOffice;

        public PassengerController(PassengerRepository passengerService, PostOffice postOffice)
        {
            _passengerService = passengerService;
            _postOffice = postOffice;
        }
        
        [HttpGet("GetAllActives")]
        public ActionResult<List<Passenger>> Get()
        {
            return Ok(_passengerService.Get());
        }

        [HttpGet("UnderAgeOnes")]
        public ActionResult<List<Passenger>> GetAllMinors()
        {
            var minors = _passengerService.GetAllMinors();
            return minors;
        }

        [HttpGet("Inactives")]
        public ActionResult<List<Passenger>> GetDeletedOnes()
        {
            return Ok(_passengerService.GetDeletedOnes());
        }

        [HttpGet("RestrictedOnes")]
        public ActionResult<List<Passenger>> GetRestrictedOnes()
        {
            return Ok(_passengerService.GetRestrictedOnes());
        }

        [HttpGet("{_id}")]
        public ActionResult<Passenger> GetByCPF(string _id)
        {
            var passenger = _passengerService.GetByCPF(_id);
            if (passenger == null) return new Passenger();
            return Ok(passenger);
        }

        [HttpPost("Create")]
        public ActionResult Post(PassengerDTO passengeDTO, int number, string complement)
        {
            var dto = _postOffice.GetAddress(passengeDTO.CEP).Result;
            if (!ValidateDocument.ValidateCPF(passengeDTO.CPF, passengeDTO.CPF)) return BadRequest("CPF Inválido!");

            Address address = new()
            {
                Number = number,
                Complement = complement,

                Street = dto.Street,
                Neighborhood = dto.Neighborhood,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode,
            };

            Passenger passenger = new(passengeDTO, address);           

            try
            {
                _passengerService.Create(passenger);
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

        [HttpPut("{id}/SetRestrict")]
        public ActionResult<Passenger> SetPassengerAsRestricted(string id)
        {
            var passenger = _passengerService.SetPassengerAsRestricted(id);
            if (passenger == null) return NotFound();

            return Ok(passenger);
        }

        [HttpPut("{id}/SetUnrestrict")]
        public ActionResult<Passenger> SetPassengerAsUnrestricted(string id)
        {
            var passenger = _passengerService.SetPassengerAsUnrestricted(id);
            if (passenger == null) return NotFound();

            return Ok(passenger);
        }

        [HttpPut("{id}/Reactivate")]
        public void ReactivatePassenger(string id)
        {
            _passengerService.ReativatePassenger(id);
        }
        
        [HttpDelete("{id}/Desactivate")]
        public void Delete(string id)
        {
            _passengerService.Delete(id);
        }
    }
}
