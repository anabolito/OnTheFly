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

        [HttpGet]
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
            if (!CnpjValidation(company.CNPJ)) return BadRequest("CPF Inválido!");

            _companyRepository.CreateCompany(company);
            return StatusCode(201);
        }

        private bool CnpjValidation(string cnpj)
        {

            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
                return false;

            int[] multiplicadores1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadores2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);

            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicadores1[i];

            int resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();

            tempCnpj += digito;

            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicadores2[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();

            return cnpj.EndsWith(digito);

        }


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
