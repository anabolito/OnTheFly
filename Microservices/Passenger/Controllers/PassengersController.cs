using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Models;
using Newtonsoft.Json;
using PassengerAPI.AddressService;
using PassengerAPI.DTO;
using PassengerAPI.Repositories;
using PassengerAPI.Service;
using System.Net;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace PassengerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengersController : ControllerBase
    {
        private readonly PassengerRepository _passengerService;
        private readonly PostOffice _postOffice;

        public PassengersController(PassengerRepository passengerService, PostOffice postOffice)
        {
            _passengerService = passengerService;
            _postOffice = postOffice;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetAllActives")]
        public ActionResult<List<Passenger>> Get()
        {
            var passengers = _passengerService.Get();
            if (passengers == null) return StatusCode((int)HttpStatusCode.NotFound, "Não existem passageiros cadastrados!");

            return Ok(passengers);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("/Passenger/Find/GetAllMinors")]
        public ActionResult<List<Passenger>> GetAllMinors()
        {
            var minors = _passengerService.GetAllMinors();
            if (minors == null) return StatusCode((int)HttpStatusCode.NotFound, "Não existem passageiros menores de idade cadastrados!");
            return Ok(minors);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("/Passenger/Find/GetAllInactives")]
        public ActionResult<List<Passenger>> GetDeletedOnes()
        {
            var inactives = _passengerService.GetDeletedOnes();
            if (inactives == null) return StatusCode((int)HttpStatusCode.NotFound, "Não existem passageiros inativos!");
            return Ok(inactives);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("/Passenger/Find/GetAllRestricted")]
        public ActionResult<List<Passenger>> GetRestrictedOnes()
        {
            var restricted =_passengerService.GetRestrictedOnes();
            if (restricted == null) return StatusCode((int)HttpStatusCode.NotFound, "Não existem passageiros restritos!");
            return Ok(restricted);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("/Passenger/Find/{cpf}")]
        public ActionResult<Passenger> GetByCPF(string cpf)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger = _passengerService.GetByCPF(cpf);
            if (passenger == null) return NotFound("Passageiro não encontrado!");

            return Ok(passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("/Passenger/Create/New")]
        public ActionResult<Passenger> Post(PassengerDTO passengerDTO)
        {
            var dto = _postOffice.GetAddress(passengerDTO.CEP).Result;
            if (!ValidateDocument.ValidateCPF(passengerDTO.CPF, passengerDTO.CPF)) return BadRequest("CPF Inválido!");            

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
                return StatusCode((int)HttpStatusCode.Created, passenger);

            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }   
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("/Passenger/Update/Address/Number/Complement/{cpf}")]
        public ActionResult<Passenger> UpdatePassengerAddress(string cpf, string cep, int number, string complement)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger = _passengerService.UpdatePassengerAddress(cpf, cep, number, complement);
            if (passenger == null) return NotFound("Passageiro não encontrado!");

            return Created(string.Empty, passenger);
        }



        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("/Passenger/Update/Street/{cpf}")]
        public ActionResult<Passenger> UpdatePassengerAddressStreet(string cpf, string streetName)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger = _passengerService.UpdatePassengerAddressStreet(cpf, streetName);
            if (passenger == null) return NotFound("Passageiro não encontrado!");

            return Created(string.Empty,passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("/Passenger/Update/Name/{cpf}")]
        public ActionResult<Passenger> UpdatePassengerName(string cpf, string name)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger = _passengerService.UpdatePassengerName(cpf, name);
            if (passenger == null) return NotFound("Passageiro não encontrado!");

            return Created(string.Empty, passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("/Passenger/Update/Gen/{cpf}")]
        public ActionResult<Passenger> UpdatePassengerGender(string cpf, char gen)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger = _passengerService.UpdatePassengerGender(cpf, gen);
            if (passenger == null) return NotFound("Passageiro não encontrado!");

            return Created(string.Empty, passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("/Passenger/Update/Phone/{cpf}")]
        public ActionResult<Passenger> UpdatePassengerPhone(string cpf, string phone)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger = _passengerService.UpdatePassengerPhone(cpf, phone);
            if (passenger == null) return NotFound("Passageiro não encontrado!");

            return Created(string.Empty, passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("/Passenger/Update/Status/{cpf}")]
        public ActionResult<Passenger> UpdatePassengerStatus(string cpf)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger = _passengerService.UpdatePassengerStatus(cpf);
            if (passenger == null) return NotFound("Passageiro não encontrado!");

            return Created(string.Empty, passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("/Passenger/Update/SetRestrict/{cpf}")]
        public ActionResult<Passenger> SetPassengerAsRestricted(string cpf)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger = _passengerService.SetPassengerAsRestricted(cpf);
            if (passenger == null) return NotFound("Passageiro não encontrado!");

            return Created(string.Empty, passenger);
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("/Passenger/Update/SetUnrestrict/{cpf}")]
        public ActionResult<Passenger> SetPassengerAsUnrestricted(string cpf)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger = _passengerService.SetPassengerAsUnrestricted(cpf);
            if (passenger == null) return NotFound("Passageiro não encontrado!");

            return Created(string.Empty, passenger);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("/Passenger/Update/Reactivate/{cpf}")]
        public ActionResult<string> ReactivatePassenger(string cpf)
        {           
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger =_passengerService.ReativatePassenger(cpf);            
            if (passenger == null) return NotFound("Passageiro não encontrado!");
            return Ok(JsonConvert.SerializeObject(passenger));
        }



        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("/Passenger/Update/Desactivate/{cpf}")]
        public ActionResult <Passenger>Delete(string cpf)
        {
            if (!ValidateDocument.ValidateCPF(cpf, cpf)) return BadRequest("CPF Inválido!");
            var passenger = _passengerService.Delete(cpf);
            if (passenger == null) return NotFound("Passageiro não encontrado!");
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}
