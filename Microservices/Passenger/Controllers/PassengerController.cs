using Microsoft.AspNetCore.Mvc;
using Models;
using PassengerAPI.AddressService;
using PassengerAPI.DTO;
using PassengerAPI.Repositories;
using PassengerAPI.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PassengerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {

        private readonly PassengerService _passengerService;
        private readonly PostOffice _postOffice;

        public PassengerController( PassengerService passengerService, PostOffice postOffice)
        {
            _passengerService = passengerService;
            _postOffice = postOffice;
        }

        // GET: api/<PassengerController>
        [HttpGet("/Passengers")]
        public ActionResult<List<Passenger>> Get() => _passengerService.Get();
       

        // GET api/<PassengerController>/5
        [HttpGet("{/CPF}")]
        public ActionResult<Passenger> Get(string cpf)
        {
            var passenger = _passengerService.Get(cpf);
            if (passenger == null) return NotFound();

            return Ok(passenger);
        }

        // POST api/<PassengerController>
        [HttpPost("{/Create}")]
        public ActionResult Post([FromBody] Passenger passenger)
        {
            if(!validateCPF.ValidateDoc(passenger.CPF)) return BadRequest("CPF Inválido!");

            _passengerService.Create(passenger);
            return StatusCode(201);
        }

        // PUT api/<PassengerController>/5
        [HttpPut("{/Update}")]
        public ActionResult Put([FromBody] string cpf, PassengerDTO passengerDTO)
        {
            var passenger = _passengerService.Get(cpf);
            if (passenger == null) return NotFound("Passageiro não encontrado");

            passenger.CPF = cpf;
            passenger.Name = passengerDTO.Name;
            passenger.Gender = passengerDTO.Gender;
            passenger.Phone = passengerDTO.Phone;
            passenger.DtBirth = passengerDTO.DtBirth;
            passenger.DtRegistry = passengerDTO.DtRegistry;
            passenger.Status = passengerDTO.Status;
            passenger.Address = passengerDTO.Address;

            return StatusCode(202);
        }


        // DELETE api/<PassengerController>/5
        [HttpDelete("{/CPF}")]
        public void Delete(string cpf)
        {
            _passengerService.Delete(cpf);
        }
    }
}
