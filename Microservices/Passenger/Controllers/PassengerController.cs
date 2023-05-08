﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Models;
using PassengerAPI.AddressService;
using PassengerAPI.DTO;
using PassengerAPI.Repositories;
using PassengerAPI.Service;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/<PassengerController>
        [HttpGet("Passengers")]
        public ActionResult<List<Passenger>> Get()
        {
            return Ok(_passengerService.Get());            

        }


        // GET api/<PassengerController>/5
        [HttpGet("{_id}")]
        public ActionResult<Passenger> Get(string _id)
        {
            var passenger = _passengerService.GetByCPF(_id);
            if (passenger == null) return new Passenger();
            return Ok(passenger);
        }

        // POST api/<PassengerController>
        [HttpPost]
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
        [HttpPut("{id}/address")]
        public ActionResult<Passenger> UpdatePassengerAddress(string id, int number, string? complement, string cep)
        {
            var passenger = _passengerService.UpdatePassengerAddress(id, number, complement, cep);
            if (passenger == null) return NotFound();

            return Ok(passenger);
        }

        // PUT api/<PassengerController>/5
        //[HttpPut("{_id}")]
        //public ActionResult Put(int number, string complement ,string _id, PassengerDTO passengerDTO)
        //{
        //    var dto = _postOffice.GetAddress(passengerDTO.CEP).Result;

        //    Address address;
        //    if (complement == null)
        //    {
        //         address = new()
        //        {
        //            Number = number,                    

        //            Street = dto.Street,
        //            Neighborhood = dto.Neighborhood,
        //            City = dto.City,
        //            State = dto.State,
        //            ZipCode = dto.ZipCode,
        //        };
        //    }
        //    else
        //    {
        //        address = new()
        //        {
        //            Number = number,

        //            Complement = complement,

        //            Street = dto.Street,
        //            Neighborhood = dto.Neighborhood,
        //            City = dto.City,
        //            State = dto.State,
        //            ZipCode = dto.ZipCode,
        //        };
        //    }            

        //    Passenger passenger = new(passengerDTO, address);
            
        //    return Ok(passenger);
        //}

        //PUT api/<PassengerController>/5
        //[HttpPut("{_id}")]
        //public async ActionResult PutAddress(string _id, int number, string? complement, PassengerDTO passengerDTO)
        //{
        //    var dto = _postOffice.GetAddress(passengerDTO.CEP).Result;
        //    var updatedPassenger = _passengerService.UpdateAddress(_id, number, complement, dto);

        //    Address address;
        //    if (complement == null)
        //    {
        //        address = new()
        //        {
        //            Number = number,

        //            Street = dto.Street,
        //            Neighborhood = dto.Neighborhood,
        //            City = dto.City,
        //            State = dto.State,
        //            ZipCode = dto.ZipCode,
        //        };
                
        //    }
        //    else
        //    {
        //        address = new()
        //        {
        //            Number = number,

        //            Complement = complement,

        //            Street = dto.Street,
        //            Neighborhood = dto.Neighborhood,
        //            City = dto.City,
        //            State = dto.State,
        //            ZipCode = dto.ZipCode,
        //        };
        //    }            

        //    if (updatedPassenger == null) return NotFound();
        //    return Ok(updatedPassenger);
        //}

        // DELETE api/<PassengerController>/5
        [HttpDelete("{DeleteCPF}")]
        public void Delete(string cpf)
        {
            _passengerService.Delete(cpf);
        }
    }
}
