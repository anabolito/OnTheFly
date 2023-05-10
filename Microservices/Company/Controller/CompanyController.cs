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
                Neighborhood = dto.Neighborhood,
                Complement = company.Address.Complement
            };
            company.Address = address;

            if (company.NameOpt == null)
            {
                company.NameOpt = company.Name;
            }

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

        [HttpGet("ReleasedCompany")]
        public ActionResult<List<Company>> GetReleasedCompany()
        {
            try
            {
                var list = _companyRepository.GetReleasedCompany();
                return StatusCode(201, list);
            }
            catch (BadHttpRequestException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("RestrictedCompany")]
        public ActionResult<List<Company>> GetRestrictedCompany()
        {
            try
            {
                var list = _companyRepository.GetRestrictedCompany();
                return StatusCode(201, list);
            }
            catch (BadHttpRequestException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("DeletedCompany")]
        public ActionResult<List<Company>> GetDeletedCompany()
        {
            try
            {
                var list = _companyRepository.GetDeletedCompany();
                return StatusCode(201, list);
            }
            catch (BadHttpRequestException ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("{cnpj}")]
        public ActionResult<Company> GetByCnpj(string cnpj)
        {
            var company = _companyRepository.GetCompanyByCnpj(cnpj);
            
            if (_companyRepository.CnpjValidation(cnpj) == false) return BadRequest("Este CNPJ não é válido, digite o CNPJ corretamente!");
            if (company == null) return NotFound("CNPJ informado não consta nos registros...");

            return StatusCode(201, company);
        }

        [HttpPut("NameOptUpdate{cnpj}")]
        public ActionResult<Company> UpdateNameOptCompany(string cnpj, string nameOpt)
        {
            var companyAux = _companyRepository.GetCompanyByCnpj(cnpj);
            if (companyAux == null) return NotFound("Companhia aérea não encontrada");

            companyAux.NameOpt = nameOpt;

            _companyRepository.UpdateCompany(cnpj, companyAux);

            return StatusCode(201, companyAux);
        }

        [HttpPut("StatusUpdate{cnpj}")]
        public ActionResult<Company> UpdateStatusCompany(string cnpj)
        {
            var companyAux = _companyRepository.GetCompanyByCnpj(cnpj);
            if (companyAux == null) return NotFound("Companhia aérea não encontrada");

            if(companyAux.Status == true)
            {
                companyAux.Status = false;
            }
            else
            {
                companyAux.Status = true;
            }

            _companyRepository.UpdateCompany(cnpj, companyAux);

            return StatusCode(201, companyAux);
        }

        [HttpPut("AddressUpdate{cnpj}")]
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

            return StatusCode(201, companyAux);
        }

        [HttpPut("StreetAddressUpdate{cnpj}")]
        public ActionResult<Company> UpdateStreetAddress(string cnpj, string street)
        {
            var companyAux = _companyRepository.GetCompanyByCnpj(cnpj);
            companyAux.Address.Street = street;

            _companyRepository.UpdateCompany(cnpj, companyAux);

            return StatusCode(201, companyAux);
        }

        [HttpPut("Restriction{cnpj}")]
        public ActionResult<Company> UpdateRestrictionCompany(string cnpj)
        {
            var companyAux = _companyRepository.GetCompanyByCnpj(cnpj);
            if (companyAux == null) return NotFound("Companhia aérea não encontrada");

            _companyRepository.UpdateRestrictionCompany(cnpj);

            return StatusCode(201, companyAux);
        }


        [HttpDelete]
        public ActionResult Delete(string cnpj)
        {
            var companyAux = _companyRepository.GetCompanyByCnpj(cnpj);

            if (companyAux == null) 
            { 
                return NotFound("Companhia aérea não encontrada"); 
            }

            _companyRepository.DeleteCompany(cnpj);
            return StatusCode(200, companyAux);
        }
    }
}
