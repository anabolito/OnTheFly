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
            if (minors == null) return StatusCode(404, "Não existem passageiros menores de idade cadastrados!");
            return minors;
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("Inactives")]
        public ActionResult<List<Passenger>> GetDeletedOnes()
        {
            var inactives = _passengerService.GetDeletedOnes();
            if (inactives == null) return StatusCode(404, "Não existem passageiros inativos!");
            return inactives;
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("RestrictedOnes")]
        public ActionResult<List<Passenger>> GetRestrictedOnes()
        {
            var restricted =_passengerService.GetRestrictedOnes();
            if (restricted == null) return StatusCode(404, "Não existem passageiros restritos!");
            return restricted;
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{cpf}")]
        public ActionResult<Passenger> GetByCPF(string cpf)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger = _passengerService.GetByCPF(cpf);
            if (passenger == null) return NotFound("Passageiro não encontrado!");

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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("/Address/number/complement/{cpf}")]
        public ActionResult<Passenger> UpdatePassengerAddress(string cpf, string cep, int number, string complement)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger = _passengerService.UpdatePassengerAddress(cpf, cep, number, complement);
            if (passenger == null) return NotFound("Passageiro não encontrado!");

            return Created(string.Empty, passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("/Street/{cpf}")]
        public ActionResult<Passenger> UpdatePassengerAddressStreet(string cpf, string streetName)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger = _passengerService.UpdatePassengerAddressStreet(cpf, streetName);
            if (passenger == null) return NotFound("Passageiro não encontrado!");

            return Ok(passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("/Name/{cpf}")]
        public ActionResult<Passenger> UpdatePassengerName(string cpf, string name)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger = _passengerService.UpdatePassengerName(cpf, name);
            if (passenger == null) return NotFound("Passageiro não encontrado!");

            return Ok(passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("/Gen/{cpf}")]
        public ActionResult<Passenger> UpdatePassengerGender(string cpf, char gen)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger = _passengerService.UpdatePassengerGender(cpf, gen);
            if (passenger == null) return NotFound("Passageiro não encontrado!");

            return Ok(passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("/Phone/{cpf}")]
        public ActionResult<Passenger> UpdatePassengerPhone(string cpf, string phone)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger = _passengerService.UpdatePassengerPhone(cpf, phone);
            if (passenger == null) return NotFound("Passageiro não encontrado!");

            return Ok(passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("/Status/{cpf}")]
        public ActionResult<Passenger> UpdatePassengerStatus(string cpf)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger = _passengerService.UpdatePassengerStatus(cpf);
            if (passenger == null) return NotFound("Passageiro não encontrado!");

            return Ok(passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("/SetRestrict/{cpf}")]
        public ActionResult<Passenger> SetPassengerAsRestricted(string cpf)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger = _passengerService.SetPassengerAsRestricted(cpf);
            if (passenger == null) return NotFound("Passageiro não encontrado!");

            return Ok(passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("/SetUnrestrict/{cpf}")]
        public ActionResult<Passenger> SetPassengerAsUnrestricted(string cpf)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger = _passengerService.SetPassengerAsUnrestricted(cpf);
            if (passenger == null) return NotFound("Passageiro não encontrado!");

            return Ok(passenger);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("/Reactivate/{cpf}")]
        public ActionResult<Passenger> ReactivatePassenger(string cpf)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger =_passengerService.ReativatePassenger(cpf);
            if (passenger == null) return NotFound("Passageiro não encontrado!");
            return Ok(passenger);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("/Desactivate/{cpf}")]
        public ActionResult <Passenger>Delete(string cpf)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger = _passengerService.Delete(cpf);
            if (passenger == null) return NotFound("Passageiro não encontrado!");
            return Ok(passenger);
        }
    }
}
