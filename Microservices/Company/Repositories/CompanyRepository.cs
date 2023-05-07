using CompanyAPI.Utils;
using Models;
using MongoDB.Driver;
using System.Net;

namespace CompanyAPI.Repositories
{
    public class CompanyRepository
    {
        private readonly IMongoCollection<Company> _company;

        public CompanyRepository(IDataBaseSettings settings)
        {
            var company = new MongoClient(settings.ConnectionString);
            var database = company.GetDatabase(settings.DataBaseName);
            _company = database.GetCollection<Company>(settings.CompanyCollectionName);
        }

        public List<Company> GetCompany() =>
            _company.Find(company => true).ToList();

        public Company CreateCompany(Company company)
        {
            _company.InsertOne(company);
            return company;
        }

        public Company GetCompanyByCnpj(string cnpj) =>
            _company.Find(company => company.CNPJ == cnpj).FirstOrDefault();

        public void UpdateCompany(string cnpj, Company company) => _company.ReplaceOne(a => a.CNPJ == cnpj, company);

        public void DeleteCompany(string cnpj) => _company.DeleteOne(a => a.CNPJ == cnpj);



    }
}
