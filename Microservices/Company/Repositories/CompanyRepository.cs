using CompanyAPI.AddressService;
using CompanyAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Models;
using MongoDB.Driver;
using System.Net;
using ZstdSharp;

namespace CompanyAPI.Repositories
{
    public class CompanyRepository
    {
        private readonly IMongoCollection<Company> _company;
        private readonly IMongoCollection<Company> _restrictedCompany;
        private readonly IMongoCollection<Company> _releasedCompany;
        public CompanyRepository(IDatabaseSettings settings)
        {
            var company = new MongoClient(settings.ConnectionString);
            var database = company.GetDatabase(settings.DataBaseName);
            _company = database.GetCollection<Company>(settings.CompanyCollectionName);
            _restrictedCompany = database.GetCollection<Company>(settings.RestrictedCompaniesCollectionName);
            _releasedCompany = database.GetCollection<Company>(settings.ReleasedCompaniesCollectionName);
        }

        public List<Company> GetCompany() =>
            _company.Find(company => true).ToList();


        public List<Company> GetRestrictedCompany() =>
    _restrictedCompany.Find(company => true).ToList();


        public List<Company> GetReleasedCompany() =>
    _releasedCompany.Find(company => true).ToList();

        public Company CreateCompany(Company company)
        {

            if (!CnpjValidation(company.CNPJ))
            {
                Console.WriteLine("CNPJ Inválido!");
                throw new BadHttpRequestException("CNPJ Inválido!");
            }
            
            _company.InsertOne(company);

            if(company.Status == true)
            {
                _releasedCompany.InsertOne(company);
            }
            else
            {
                _restrictedCompany.InsertOne(company);

            }
            return company;
        }


        public Company GetCompanyByCnpj(string cnpj) =>
            _company.Find(company => company.CNPJ == cnpj).FirstOrDefault();

        public void UpdateCompany(string cnpj, Company company)
        {
            Company companyAux = new();
            companyAux = _company.Find(companyAux => companyAux.CNPJ == cnpj).FirstOrDefault();
            var status = companyAux.Status;

            _company.ReplaceOne(a => a.CNPJ == cnpj, company);
           
            if(status == true && company.Status == false)
            {
                _releasedCompany.DeleteOne(a => a.CNPJ == cnpj);
                _restrictedCompany.InsertOne(company);
            }
            if (status == false && company.Status == true)
            {
                _restrictedCompany.DeleteOne(a => a.CNPJ == cnpj);
                _releasedCompany.InsertOne(company);
            }
        }

        public void DeleteCompany(string cnpj) => _company.DeleteOne(a => a.CNPJ == cnpj);



        public bool CnpjValidation(string cnpj)
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


    }
}
