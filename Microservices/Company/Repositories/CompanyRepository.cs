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
        //private readonly IMongoCollection<Company> _company;
        private readonly IMongoCollection<Company> _restrictedCompany;
        private readonly IMongoCollection<Company> _releasedCompany;
        private readonly IMongoCollection<Company> _deletedCompany;
        public CompanyRepository(IDatabaseSettings settings)
        {
            var company = new MongoClient(settings.ConnectionString);
            var database = company.GetDatabase(settings.DataBaseName);
            //_company = database.GetCollection<Company>(settings.CompanyCollectionName);
            _restrictedCompany = database.GetCollection<Company>(settings.RestrictedCompaniesCollectionName);
            _releasedCompany = database.GetCollection<Company>(settings.ReleasedCompaniesCollectionName);
            _deletedCompany = database.GetCollection<Company>(settings.DeletedCompaniesCollectionName);

        }

        public Company CreateCompany(Company company)
        {

            if (!CnpjValidation(company.CNPJ))
            {
                Console.WriteLine("CNPJ Inválido!");
                throw new BadHttpRequestException("CNPJ Inválido!");
            }

            _releasedCompany.InsertOne(company);

            return company;
        }
        public List<Company> GetReleasedCompany()
        {
            if (_releasedCompany.Find(company => true).ToList().Count == 0)
            {
                throw new BadHttpRequestException("Não há companhias aéreas liberadas.");
            }
            return _releasedCompany.Find(company => true).ToList();
        }

        public List<Company> GetRestrictedCompany()
        {
            if (_restrictedCompany.Find(company => true).ToList().Count == 0)
            {
                throw new BadHttpRequestException("Não há companhias aéreas restritas.");
            }
            return _restrictedCompany.Find(company => true).ToList();
        }

        public List<Company> GetDeletedCompany()
        {
            if (_deletedCompany.Find(company => true).ToList().Count == 0)
            {
                throw new BadHttpRequestException("Não há companhias aéreas deletadas.");
            }

            return _deletedCompany.Find(company => true).ToList();
        }

        public Company GetCompanyByCnpj(string cnpj)
        {
            var company = _releasedCompany.Find(a => a.CNPJ == cnpj).FirstOrDefault();
            
            if(company == null)
            {
                company = _restrictedCompany.Find(a => a.CNPJ == cnpj).FirstOrDefault();
            }

            return company;
        }

        public void UpdateCompany(string cnpj, Company company)
        {
            Company companyAux = new();
            companyAux = _releasedCompany.Find(companyAux => companyAux.CNPJ == cnpj).FirstOrDefault();
            var status = companyAux.Status;

            _releasedCompany.ReplaceOne(a => a.CNPJ == cnpj, company);
        }

        public bool UpdateRestrictionCompany(string cnpj)
        {
            var companyAux = _releasedCompany.Find(companyAux => companyAux.CNPJ == cnpj).FirstOrDefault();
            if (companyAux == null)
            {
                companyAux = _restrictedCompany.Find(companyAux => companyAux.CNPJ == cnpj).FirstOrDefault();
                _releasedCompany.InsertOne(companyAux);
                _restrictedCompany.DeleteOne(companyAux => companyAux.CNPJ == cnpj);
            }

            else
            {
                _restrictedCompany.InsertOne(companyAux);
                _releasedCompany.DeleteOne(companyAux => companyAux.CNPJ == cnpj);
            }
            return true;
        }
        public void DeleteCompany(string cnpj)
        {
            var company = _releasedCompany.Find(a => a.CNPJ == cnpj).FirstOrDefault();
            if(company == null) 
            { 
            company = _restrictedCompany.Find(a => a.CNPJ == cnpj).FirstOrDefault();
            }
            _deletedCompany.InsertOne(company);
            _releasedCompany.DeleteOne(a => a.CNPJ == cnpj);
            _restrictedCompany.DeleteOne(a => a.CNPJ == cnpj);
        }

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
