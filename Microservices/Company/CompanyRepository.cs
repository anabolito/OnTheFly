
using Company.Utils;
using Models;
using MongoDB.Driver;

namespace Company
{
    public class CompanyRepository
    {
        private readonly IMongoCollection<CompanyModel> _companies;

        public CompanyRepository(IDataBaseSettings settings)
        {
            var company = new MongoClient(settings.ConnectionString);
            var database = company.GetDatabase(settings.DataBaseName);
            _companies = database.GetCollection<CompanyModel>(settings.CompanyCollectionName);
        }

        public List<CompanyModel> Get() =>
            _companies.Find(company => true).ToList();

        public CompanyModel Get(string cnpj) =>
            _companies.Find<CompanyModel>(company => company.CNPJ == cnpj).FirstOrDefault();

        //public Airport GetByIcao(string icao) =>
        //    _airports.Find<Airport>(airport => airport.icao == icao).FirstOrDefault();

        //public List<Airport> GetByState(string state) =>
        //    _airports.Find<Airport>(airport => airport.state == state).ToList();
        //public List<Airport> GetByCityCode(string city_code) =>
        //    _airports.Find<Airport>(airport => airport.city_code == city_code).ToList();

        //public List<Airport> GetByCityName(string city) =>
        //    _airports.Find<Airport>(airport => airport.city == city).ToList();

        //public List<Airport> GetByCountry(string country_id) =>
        //    _airports.Find<Airport>(airport => airport.country_id == country_id).ToList();
    }
}
