using CompanyAPI.AddressService;
using CompanyAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using MongoDB.Driver;
using System.Net;


namespace CompanyAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyRepository _companyRepository;
        private readonly PostOfficeService _postOfficeService;

        public CompanyController(CompanyRepository companyRepository, PostOfficeService postOfficeService)
        {
            _companyRepository = companyRepository;
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
                _companyRepository.CreateCompany(company);
                return StatusCode(201);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("COMPANHIAS AÉREAS")]
        public ActionResult<List<Company>> GetCompany() => _companyRepository.GetCompany();

        [HttpGet("COMPANHIAS AÉREAS RESTRITAS")]
        public ActionResult<List<Company>> GetRestrictedCompany() => _companyRepository.GetRestrictedCompany();

        [HttpGet("COMPANHIAS AÉREAS LIBERADAS")]
        public ActionResult<List<Company>> GetReleasedCompany() => _companyRepository.GetReleasedCompany();

        [HttpGet("{cnpj}")]
        public ActionResult<Company> Get(string cnpj)
        {
            var company = _companyRepository.GetCompanyByCnpj(cnpj);

            if (company == null) return NotFound();
            return company;
        }

        [HttpPut("{cnpj} Modificar Nome Fantasia")]
        public ActionResult<Company> UpdateNameOptCompany(string cnpj, string nameOpt)
        {
            var companyAux = _companyRepository.GetCompanyByCnpj(cnpj);
            if (companyAux == null) return NotFound("Companhia aérea não encontrada");

            companyAux.NameOpt = nameOpt;

            _companyRepository.UpdateCompany(cnpj, companyAux);

            return StatusCode(202);
        }

        [HttpPut("{cnpj} Modificar Status da Companhia Aérea")]
        public ActionResult<Company> UpdateStatusCompany(string cnpj, Company company)
        {
            var companyAux = _companyRepository.GetCompanyByCnpj(cnpj);
            if (companyAux == null) return NotFound("Companhia aérea não encontrada");

            companyAux.Status = company.Status;

            _companyRepository.UpdateCompany(cnpj, companyAux);

            return StatusCode(202);
        }

        [HttpPut("{cnpj} Modificar Endereço da Companhia Aérea")]
        public ActionResult<Company> UpdateAddressCompany(string cnpj, Address address)
        {
            var companyAux = _companyRepository.GetCompanyByCnpj(cnpj);

            var dto = _postOfficeService.GetAddress(companyAux.Address.ZipCode).Result;

            address.Street = dto.Street;
            address.Number = companyAux.Address.Number;
            address.State = dto.State;
            address.ZipCode = dto.ZipCode;
            address.City = dto.City;
            address.Complement = companyAux.Address.Complement;
            
            companyAux.Address = address;

            _companyRepository.UpdateCompany(cnpj, companyAux);

            return StatusCode(202);
        }

        [HttpDelete]
        public ActionResult Delete(string cnpj)
        {
            if (cnpj == null) return NotFound();
            var address = _companyRepository.GetCompanyByCnpj(cnpj);

            if (address == null) return NotFound();

            _companyRepository.DeleteCompany(cnpj);
            return Ok();
        }
    }
}
