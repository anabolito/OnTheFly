using CompanyAPI.Utils;
using Models;
using MongoDB.Driver;

namespace CompanyAPI.Repositories
{
    public class CompanyRepository
    {
        private readonly IMongoCollection<Company> _companies;

        public CompanyRepository(IDataBaseSettings settings)
        {
            var company = new MongoClient(settings.ConnectionString);
            var database = company.GetDatabase(settings.DataBaseName);
            _companies = database.GetCollection<Company>(settings.CompanyCollectionName);
        }

        public List<Company> Get() =>
            _companies.Find(company => true).ToList();

        public Company Get(string cnpj) =>
            _companies.Find(company => company.CNPJ == cnpj).FirstOrDefault();



    }
}
