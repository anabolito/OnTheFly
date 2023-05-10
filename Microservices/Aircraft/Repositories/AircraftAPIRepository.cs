using System.Net;
using AircraftAPI.Config;
using Microsoft.AspNetCore.Mvc;
using Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Services;

namespace AircraftAPI.Services
{
    public class AircraftAPIRepository
    {

        private readonly IMongoCollection<Aircraft> _aircraft;
        private readonly IMongoCollection<Aircraft> _deletedAircraft;
        private readonly CompanyService _companyService;

        public AircraftAPIRepository(IAircraftAPISettings settings, CompanyService companyService)
        {
            var aircraft = new MongoClient(settings.ConnectionString);
            var database = aircraft.GetDatabase(settings.DatabaseName);
            _aircraft = database.GetCollection<Aircraft>(settings.AircraftCollectionName);
            _deletedAircraft = database.GetCollection<Aircraft>(settings.DeletedAircraftCollectionName);

            _companyService = companyService;
        }


        public List<Aircraft> Get()
        {
            if (_aircraft.Find(c => true).ToList().Count == 0)
            {
                throw new BadHttpRequestException("Não há aeronaves cadastradas.");
            }
            var list = new List<Aircraft>();
            list = _aircraft.Find(c => true).ToList();
            return list;        
        }

        public int GetCapacity(string id)
        {
            if(_aircraft.Find<Aircraft>(c => c.RAB == id).FirstOrDefault() == null)
            {
                throw new BadHttpRequestException("Aeronave não encontrada.");
            }

            Aircraft a = new Aircraft();
            a = _aircraft.Find<Aircraft>(c => c.RAB == id).FirstOrDefault();

            return a.Capacity;
        }

        public Aircraft Get(string id)
        {
            if (_aircraft.Find<Aircraft>(c => c.RAB == id).FirstOrDefault() == null)
            {
                throw new BadHttpRequestException("Aeronave não encontrada.");
            }
            var aircraft = new Aircraft();
            aircraft = _aircraft.Find<Aircraft>(c => c.RAB == id).FirstOrDefault();
            return aircraft;
        }

        public async Task<Aircraft> Create(Aircraft aircraft)
        {
            
            Company company = new Company();
            company = await _companyService.GetByCnpj(aircraft.Company.CNPJ);
            aircraft.Company = company;

            if (company != null)
            {
                if (aircraft.Company.Status == true)
                {
                    if (ValidateRAB(aircraft.RAB))
                    {
                        string s = aircraft.RAB.ToUpper();
                        aircraft.RAB = s;
                        _aircraft.InsertOne(aircraft);
                        return  aircraft;
                    }
                }
            }

            return null;
        }

        public ActionResult UpdateDtLastFlight(string id, Aircraft aircraft)
        {
            var c = _aircraft.Find<Aircraft>(c => c.RAB == id).FirstOrDefault();

            if (c == null)
            {
                throw new BadHttpRequestException("Aeronave não encontrada.");
            }

            c.DtLastFlight = aircraft.DtLastFlight;

            _aircraft.ReplaceOne(c => c.RAB == aircraft.RAB, c);
            return new OkResult();
        }

        public async Task<Aircraft> UpdateCompany(string rab, Aircraft aircraft)
        {
            var c = _aircraft.Find<Aircraft>(c => c.RAB == rab).FirstOrDefault();

            if (c == null)
            {
                 throw new BadHttpRequestException("Aeronave não encontrada.");
            }

            //Company company = new Company();
            var company = await _companyService.GetByCnpj(aircraft.Company.CNPJ);
            c.Company = company;

            if (c.Company.Status == true)
            {
               _aircraft.ReplaceOne(c => c.RAB == aircraft.RAB, c);
                return c;
            }

            return null;
        }

        public void Delete(string id)
        {
            if (id == null)
            {
                throw new BadHttpRequestException("Aeronave não encontrada.");
            }
            var aircraft = _aircraft.Find<Aircraft>(c => c.RAB == id).FirstOrDefault();
            if (aircraft == null)
            {
                throw new BadHttpRequestException("Aeronave não encontrada.");
            }
            _deletedAircraft.InsertOne(aircraft);
            _aircraft.DeleteOne(c => c.RAB == id);
            
        }

        // metodos de validacao do numero RAB
        bool ValidateRAB(string st)
        {
            bool status2 = true;
            string s = st.ToUpper();

            if (s.Length != 6)
            {
                return false;
            }

            if (s[2] != '-')
            {
                return false;
            }

            if ((s[3] == 'Q') || (s[4] == 'W'))
            {
                return false;
            }

            if (!((s[0] == 'P') & (s[1] == 'T')))
            {
                if (!((s[0] == 'P') & (s[1] == 'R')))
                {
                    if (!((s[0] == 'P') & (s[1] == 'P')))
                    {
                        if (!((s[0] == 'P') & (s[1] == 'S')))
                        {
                            if (!((s[0] == 'P') & (s[1] == 'U')))
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            if (((s[3] == 'S') & (s[4] == 'O') & (s[5] == 'S'))) return false;
            if (((s[3] == 'X') & (s[4] == 'X') & (s[5] == 'X'))) return false;
            if (((s[3] == 'P') & (s[4] == 'A') & (s[5] == 'N'))) return false;
            if (((s[3] == 'T') & (s[4] == 'T') & (s[5] == 'T'))) return false;
            if (((s[3] == 'V') & (s[4] == 'F') & (s[5] == 'R'))) return false;
            if (((s[3] == 'I') & (s[4] == 'F') & (s[5] == 'R'))) return false;
            if (((s[3] == 'V') & (s[4] == 'M') & (s[5] == 'C'))) return false;
            if (((s[3] == 'I') & (s[4] == 'M') & (s[5] == 'C'))) return false;

            return status2;
        }
    }
}
