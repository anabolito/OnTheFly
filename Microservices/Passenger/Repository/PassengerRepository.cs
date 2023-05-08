using PassengerAPI.Utils;
using Models;
using MongoDB.Driver;
using PassengerAPI.DTO;
using PassengerAPI.AddressService;

namespace PassengerAPI.Repositories
{
    public class PassengerRepository
    {
        private readonly IMongoCollection<Passenger> _passenger;
        private readonly IMongoCollection<Passenger> _unactivatedPassenger;
        private readonly IMongoCollection<Passenger> _restrictedPassenger;

        public PassengerRepository(IPassengerSettings settings)
        {
            var passenger = new MongoClient(settings.ConnectionString);
            var database = passenger.GetDatabase(settings.DatabaseName);

            _passenger = database.GetCollection<Passenger>(settings.PassengerCollectionName);
            _unactivatedPassenger = database.GetCollection<Passenger>(settings.InactivePassengerCollectionName);
            _restrictedPassenger = database.GetCollection<Passenger>(settings.RestrictPassengerCollectionName);
        }

        #region[C]
        public Passenger Create(Passenger passenger)
        {
            if (!(bool)passenger.Status)
                _restrictedPassenger.InsertOne(passenger);
            else
                _passenger.InsertOne(passenger);

            return passenger;
        }
        #endregion
        #region[R]
        public List<Passenger> Get()
        {
            var allPassengers = new List<Passenger>();
            var normalPassengers = _passenger.Find(p => true).ToList();
            var restrictedPassengers = _restrictedPassenger.Find(p => true).ToList();

            allPassengers.AddRange(normalPassengers);
            allPassengers.AddRange(restrictedPassengers);

            return allPassengers;
        }

        public List<Passenger> GetCustomPassenger()
        {
            var passenger = _passenger.Find(u => true).ToList();
            return passenger;
        }

        public List<Passenger> GetUnderAgeOnes()
        {
            var underAgeOnes = _passenger.Find(u => true).ToList();
            return underAgeOnes;
        }

        public List<Passenger> GetRestrictedOnes()
        {
            var restrictedOnes = _restrictedPassenger.Find(r => true).ToList();
            return restrictedOnes;
        }

        public Passenger GetByCPF(string _id)
        {
            var customPassenger = _passenger.Find(passenger => passenger.CPF == _id).FirstOrDefault();
            var restrictedPassenger = _restrictedPassenger.Find(passenger => passenger.CPF == _id).FirstOrDefault();

            if (customPassenger != null)
                return customPassenger;
            else
                return restrictedPassenger;

        }
        #endregion
        #region[U]
        public Passenger UpdatePassengerAddress(string _id, int number, string? complement, string cep)
        {
            var passenger = _passenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();
            var restrictedPassenger = _restrictedPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();
            PostOffice _postOffice = new();
            var dto = _postOffice.GetAddress(cep).Result;
            if (passenger != null)
            {
                var address = new Address()
                {
                    Number = number,
                    Complement = complement,
                    Street = dto.Street,
                    Neighborhood = dto.Neighborhood,
                    City = dto.City,
                    State = dto.State,
                    ZipCode = dto.ZipCode
                };
                passenger.Address = address;
                _passenger.ReplaceOne(p => p.CPF == _id, passenger);
                return passenger;
            }
            else if (restrictedPassenger != null)
            {
                var address = new Address()
                {
                    Number = number,
                    Complement = complement,
                    Street = dto.Street,
                    Neighborhood = dto.Neighborhood,
                    City = dto.City,
                    State = dto.State,
                    ZipCode = dto.ZipCode
                };
                restrictedPassenger.Address = address;
                _restrictedPassenger.ReplaceOne(p => p.CPF == _id, restrictedPassenger);
                return restrictedPassenger;
            }
            return null;
        }

        //public Passenger UpdateAddress(string _id, int number, string complement, AddressDTO dto)
        //{

        //    if (string.IsNullOrEmpty(_id)) return null;


        //    if (passenger != null)
        //    {
        //        passenger.Address.ZipCode = newCEP;
        //        _passenger.ReplaceOne(p => p.CPF == _id, passenger);

        //        return passenger;
        //    }
        //    else if (restrictedPassenger != null)
        //    {
        //        restrictedPassenger.Address.ZipCode = newCEP;
        //        _restrictedPassenger.ReplaceOne(p => p.CPF == _id, passenger);

        //        return restrictedPassenger;
        //    }
        //    else return null;
        //}


        //public Passenger UpdateAddress(string _id, string cep, int number , string complement)
        //{
        //    var currentPassenger = _passenger.Find(p => p.CPF == _id).FirstOrDefault();            
        //    var restrictedPassenger = _restrictedPassenger.Find(p => p.CPF == _id).FirstOrDefault();
        //    var unactivatedPassenger = _unactivatedPassenger.Find(p => p.CPF == _id).FirstOrDefault();

        //    if (currentPassenger != null)
        //    {  
        //        //Colocando na colecao de restritos
        //        if ((bool)!passenger.Status && (bool)currentPassenger.Status)
        //        {

        //            _restrictedPassenger.InsertOne(currentPassenger);
        //            _passenger.DeleteOne(p => p.CPF == _id);
        //            return passenger;
        //        }                           
        //        else
        //        {
        //            _passenger.ReplaceOne(p => p.CPF == _id, passenger);
        //            return passenger;
        //        }                
        //    }            
        //    if(restrictedPassenger != null)
        //    {
        //        //colocando na coleção comum
        //        if ((bool)passenger.Status && (bool)!restrictedPassenger.Status)
        //        {

        //            _passenger.InsertOne(passenger);
        //            _restrictedPassenger.DeleteOne(r => r.CPF == _id);
        //        }
        //        else 
        //        { 
        //            _passenger.ReplaceOne(p => p.CPF == _id, passenger);
        //            return passenger;
        //        }

        //    }
        //    return passenger;

        //}
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

