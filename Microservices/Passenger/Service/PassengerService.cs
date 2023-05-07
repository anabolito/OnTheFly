using PassengerAPI.Utils;
using Models;
using MongoDB.Driver;

namespace PassengerAPI.Repositories
{
    public class PassengerService
    {
        private readonly IMongoCollection<Passenger> _passenger;
        private readonly IMongoCollection<Passenger> _unactivatedPassenger;
        private readonly IMongoCollection<Passenger> _underAgePassenger;
        private readonly IMongoCollection<Passenger> _restrictedPassenger;

        public PassengerService(IPassengerSettings settings)
        {
            var passenger = new MongoClient(settings.ConnectionString);
            var database = passenger.GetDatabase(settings.DatabaseName);
            _passenger = database.GetCollection<Passenger>(settings.PassengerCollectionName);
            _unactivatedPassenger = database.GetCollection<Passenger>(settings.InactivePassengerCollectionName);
            _underAgePassenger = database.GetCollection<Passenger>(settings.UnderagePassengerCollectionName);
            _restrictedPassenger = database.GetCollection <Passenger>(settings.RestrictPassengerCollectionName);
        }

        #region[C]
        public Passenger Create(Passenger passenger)
        {
            if (CalculateAge(passenger.DtBirth) < 18)
                _underAgePassenger.InsertOne(passenger);
            else if (!(bool)passenger.Status)
                _restrictedPassenger.InsertOne(passenger);
            else
                _passenger.InsertOne(passenger); 
           
            return passenger;
        }
        #endregion
        #region[R]
        public List<Passenger> Get() =>
            _passenger.Find(passenger => true).ToList();

        public Passenger Get(string cpf) =>
            _passenger.Find<Passenger>(passenger => passenger.CPF == cpf).FirstOrDefault();

        public List<Passenger> GetUnderAgeOnes()
        {
            var underAgeOnes =_underAgePassenger.Find( u => true).ToList();
            return underAgeOnes;
        }

        public List<Passenger> GetRestrictedOnes()
        {
            var restrictedOnes = _restrictedPassenger.Find(r => true).ToList();
            return restrictedOnes;
        }

        public Passenger GetByCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) return new Passenger();
            return _passenger.Find<Passenger>(x => x.CPF == cpf).FirstOrDefault();
        }
        #endregion
        #region[U]
        public Passenger Update(string cpf, Passenger passenger)
        {
            var currentPassenger = _passenger.Find(p => p.CPF == cpf).FirstOrDefault();

            if (currentPassenger != null)
            {
                //To False
                if ((bool)!passenger.Status && (bool)currentPassenger.Status)
                {
                    _restrictedPassenger.InsertOne(currentPassenger);
                    _passenger.DeleteOne(p => p.CPF == cpf);
                    return new Passenger();
                }
                //To true
                else if((bool)passenger.Status && (bool)!currentPassenger.Status)
                {
                    _passenger.InsertOne(passenger);
                    _restrictedPassenger.DeleteOne(r => r.CPF == cpf);
                    _passenger.ReplaceOne(p => p.CPF == cpf, passenger);
                    return passenger;
                }
                //To minor
                else if(CalculateAge(passenger.DtBirth) < 18 && CalculateAge(currentPassenger.DtBirth) > 18)
                {
                    _underAgePassenger.InsertOne(passenger);
                    _passenger.DeleteOne(u => u.CPF == cpf);
                    return passenger;
                }
                //To adult
                else if (CalculateAge(passenger.DtBirth) > 18 && CalculateAge(currentPassenger.DtBirth) < 18)
                {
                    _passenger.InsertOne(passenger);
                    _underAgePassenger.DeleteOne(u => u.CPF == cpf);
                    _passenger.ReplaceOne(u => u.CPF == cpf, passenger);
                    return passenger;
                }
                else 
                {
                    _passenger.ReplaceOne(p => p.CPF == cpf, passenger);
                    return passenger;

                }               
            }
            else return new Passenger();

        }
        #endregion
        #region[D]
        public async Task<Passenger> Delete(string cpf)
        {
            var passengerToDelete = await _passenger.Find<Passenger>(passenger => passenger.CPF == cpf).FirstOrDefaultAsync();
            if (passengerToDelete == null) return new Passenger();

            await _unactivatedPassenger.InsertOneAsync(passengerToDelete);
            await _passenger.DeleteOneAsync(p => p.CPF == cpf);
            return passengerToDelete;
        }
        #endregion

        private int CalculateAge(DateTime bd)
        {
            var today = DateTime.Today;
            var age = today.Year - bd.Year;
            if (bd.Date > today.AddYears(-age)) age--;
            return age;
        }      
    }
}

