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

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetAllActives")]
        public ActionResult<List<Passenger>> Get()
        {
            var passengers = _passengerService.Get();
            if (passengers == null) return StatusCode(404, "Não existem passageiros cadastrados!");

            return Ok(passengers);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("UnderAgeOnes")]
        public ActionResult<List<Passenger>> GetAllMinors()
        {
            var minors = _passengerService.GetAllMinors();
            return minors;
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("Inactives")]
        public ActionResult<List<Passenger>> GetDeletedOnes()
        {
            return Ok(_passengerService.GetDeletedOnes());
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("RestrictedOnes")]
        public ActionResult<List<Passenger>> GetRestrictedOnes()
        {
            return Ok(_passengerService.GetRestrictedOnes());
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("CPF")]
        public ActionResult<Passenger> GetByCPF(string cpf)
        {
            var passenger = _passengerService.GetByCPF(cpf);
            if (passenger == null) return new Passenger();
            return Ok(passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("Create")]
        public ActionResult<Passenger> Post(PassengerDTO passengerDTO)
        {
            var dto = _postOffice.GetAddress(passengerDTO.CEP).Result;

            if (!ValidateDocument.ValidateCPF(passengerDTO.CPF, passengerDTO.CPF)) return BadRequest("CPF Inválido!");

            var number = 0;
            var complement = "";

            Address address = new()
            {
                Number = passengerDTO.Number,
                Complement = passengerDTO.Complement,
                Street = dto.Street,
                Neighborhood = dto.Neighborhood,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode,
            };

            Passenger passenger = new(passengerDTO, address);           

            try
            {
                _passengerService.Create(passenger);
                return StatusCode(201, passenger);

            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }   
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("/Address/CPF")]
        public ActionResult<Passenger> UpdatePassengerAddress(string cpf, string cep)
        {
            var passenger = _passengerService.UpdatePassengerAddress(cpf, cep);
            if (passenger == null) return NotFound();

            return Created(string.Empty, passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("/Street/CPF")]
        public ActionResult<Passenger> UpdatePassengerAddressStreet(string cpf, string streetName)
        {
            var passenger = _passengerService.UpdatePassengerAddressStreet(cpf, streetName);
            if (passenger == null) return NotFound();

            return Ok(passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("/Name/CPF")]
        public ActionResult<Passenger> UpdatePassengerName(string cpf, string name)
        {
            var passenger = _passengerService.UpdatePassengerName(cpf, name);
            if (passenger == null) return NotFound();

            return Ok(passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("/Gen/CPF")]
        public ActionResult<Passenger> UpdatePassengerGender(string cpf, char gen)
        {
            var passenger = _passengerService.UpdatePassengerGender(cpf, gen);
            if (passenger == null) return NotFound();

            return Ok(passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("/Phone/CPF")]
        public ActionResult<Passenger> UpdatePassengerPhone(string cpf, string phone)
        {
            var passenger = _passengerService.UpdatePassengerPhone(cpf, phone);
            if (passenger == null) return NotFound();

            return Ok(passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("/Status/{CPF}")]
        public ActionResult<Passenger> UpdatePassengerStatus(string cpf)
        {
            var passenger = _passengerService.UpdatePassengerStatus(cpf);
            if (passenger == null) return NotFound();

            return Ok(passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("/SetRestrict/{CPF}")]
        public ActionResult<Passenger> SetPassengerAsRestricted(string cpf)
        {
            var passenger = _passengerService.SetPassengerAsRestricted(cpf);
            if (passenger == null) return NotFound();

            return Ok(passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("/SetUnrestrict/{CPF}")]
        public ActionResult<Passenger> SetPassengerAsUnrestricted(string cpf)
        {
            var passenger = _passengerService.SetPassengerAsUnrestricted(cpf);
            if (passenger == null) return NotFound();

            return Ok(passenger);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("/Reactivate/{CPF}")]
        public ActionResult<Passenger> ReactivatePassenger(string cpf)
        {
            var passenger =_passengerService.ReativatePassenger(cpf);
            if (passenger == null) return NotFound();
            return Ok(passenger);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("/Desactivate/{CPF}")]
        public ActionResult <Passenger>Delete(string cpf)
        {
            var passenger = _passengerService.Delete(cpf);
            if (passenger == null) return NotFound();
            return Ok(passenger);
        }
    }
}
