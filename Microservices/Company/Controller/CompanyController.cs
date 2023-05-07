using CompanyAPI.AddressService;
using CompanyAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
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

        [HttpGet]
        public ActionResult<List<Company>> GetCompany() => _companyRepository.GetCompany();

        [HttpGet("{cnpj}")]
        public ActionResult<Company> Get(string cnpj)
        {
            var company = _companyRepository.GetCompanyByCnpj(cnpj);

            if (company == null) return NotFound();
            return company;
        }

        [HttpPut("{cnpj}")]
        public ActionResult Put(string cnpj, Company company)
        {
            var companyAux = _companyRepository.GetCompanyByCnpj(cnpj);
            if (company == null) return NotFound("Companhia aérea não encontrada");

            companyAux.CNPJ = company.CNPJ;
            companyAux.Name = company.Name;
            companyAux.NameOpt = company.NameOpt;
            companyAux.DtOpen = company.DtOpen;
            companyAux.Status = company.Status;
            companyAux.Address = company.Address;

            return StatusCode(202);
        }


        [HttpPost]
        public ActionResult PostCompany(Company company)
        {

            _companyRepository.CreateCompany(company);
            return StatusCode(201);
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
