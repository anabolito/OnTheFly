using CompanyAPI.AddressService;
using CompanyAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace OnTheFly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyService _companyService;
        private readonly PostOfficeService _postOfficeService;

        public CompanyController(CompanyService companyService, PostOfficeService postOfficeService)
        {
            _companyService = companyService;
            _postOfficeService = postOfficeService;
        }

        [HttpPost]
        public ActionResult PostCompany(Company company)
        {

            var dto = _postOfficeService.GetAddress(company.Address.ZipCode).Result;

            Address address = new()
            {
                Street = dto.Street,
                Number = company.Address.Number,
                State = dto.State,
                ZipCode = dto.ZipCode,
                City = dto.City,
                Complement = company.Address.Complement
            };
            company.Address = address;

            try
            {
                _companyService.Insert(company);
                return StatusCode(201);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("COMPANHIAS AÉREAS")]
        public ActionResult<List<Company>> GetCompany() => _companyService.Get().Result;

        [HttpGet("COMPANHIAS AÉREAS RESTRITAS")]
        public ActionResult<List<Company>> GetRestrictedCompany() => _companyService.GetRestrictedCompany().Result;

        [HttpGet("COMPANHIAS AÉREAS LIBERADAS")]
        public ActionResult<List<Company>> GetReleasedCompany() => _companyService.GetReleasedCompany().Result;

        [HttpGet("{cnpj}")]
        public ActionResult<Company> Get(string cnpj)
        {
            var company = _companyService.GetByCnpj(cnpj);

            if (company == null) return NotFound();
            return company.Result;
        }

        [HttpPut("{cnpj} Modificar Nome Fantasia")]
        public ActionResult<Company> UpdateNameOptCompany(string cnpj, string nameOpt)
        {
            Company companyAux = new();
            companyAux = _companyService.GetByCnpj(cnpj).Result;

            if (companyAux == null) return NotFound("Companhia aérea não encontrada");
            

            companyAux.NameOpt = nameOpt;

            _companyService.UpdateNameOptCompany(cnpj, companyAux.NameOpt);

            return StatusCode(202);
        }

        [HttpPut("{cnpj} Modificar Status da Companhia Aérea")]
        public ActionResult<Company> UpdateStatusCompany(string cnpj, Company company)
        {
            Company companyAux = new();
            companyAux = _companyService.GetByCnpj(cnpj).Result;
            if (companyAux == null) return NotFound("Companhia aérea não encontrada");

            companyAux.Status = company.Status;

            _companyService.UpdateStatusCompany(cnpj, companyAux.Status);

            return StatusCode(202);
        }

        [HttpPut("{cnpj} Modificar Endereço da Companhia Aérea")]
        public ActionResult<Company> UpdateAddressCompany(string cnpj, Address address)
        {
            Company companyAux = new();

            companyAux = _companyService.GetByCnpj(cnpj).Result;

            var dto = _postOfficeService.GetAddress(companyAux.Address.ZipCode).Result;

            address.Street = dto.Street;
            address.Number = companyAux.Address.Number;
            address.State = dto.State;
            address.ZipCode = dto.ZipCode;
            address.City = dto.City;
            address.Complement = companyAux.Address.Complement;

            companyAux.Address = address;

            _companyService.UpdateAddressCompany(cnpj);

            return StatusCode(202);
        }

        [HttpDelete]
        public ActionResult Delete(string cnpj)
        {
            if (cnpj == null) return NotFound();
            var address = _companyService.GetByCnpj(cnpj);

            if (address == null) return NotFound();

            _companyService.Delete(cnpj);
            return Ok();
        }
    }
}

