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

        public CompanyController(CompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [HttpGet]
        public ActionResult<List<Company>> GetCompany() => _companyRepository.GetCompany();

        [HttpGet("{cnpj}")]
        public ActionResult<List<Flight>> GetCompany(string cnpj) => _companyRepository.GetCompany(cnpj);


        //[HttpPut("{cnpj}")]
        //public  ActionResult<Company> PutCompany

        [HttpPost]
        public ActionResult<Company> PostCompany(Company company)
        {
            if (_companyRepository.Company == null)
            {
                return Problem("Entity set 'AndreTurismoAppClientServiceContext.Client'  is null.");
            }
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async ActionResult<Company> DeleteCompany(int cnpj)
        {
            if (_companyRepository.Company == null)
            {
                return NotFound();
            }
            var company = await _companyRepository.Company.FindAsync(cnpj);
            if (company == null)
            {
                return NotFound();
            }

            _companyRepository.Company.Remove(company);
            await _companyRepository.Company();

            return NoContent();
        }
    }
}
