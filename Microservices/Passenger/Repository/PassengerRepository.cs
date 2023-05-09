using PassengerAPI.Utils;
using Models;
using MongoDB.Driver;
using PassengerAPI.DTO;
using PassengerAPI.AddressService;
using System.Security.Cryptography;

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

        public List<Passenger> GetDeletedOnes()
        {
            var deletedOnes = _unactivatedPassenger.Find(u => true).ToList();
            return deletedOnes;
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

        public Passenger UpdatePassengerName(string _id, string name)
        {
            var passenger = _passenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();
            var restrictedPassenger = _restrictedPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();

            if (passenger != null)
            {
                passenger.Name = name;

                _passenger.ReplaceOne(p => p.CPF == _id, passenger);
                return passenger;
            }
            else if (restrictedPassenger != null)
            {

                passenger.Name = name;

                _restrictedPassenger.ReplaceOne(p => p.CPF == _id, passenger);
                return restrictedPassenger;
            }
            return null;
        }

        public Passenger UpdatePassengerGender(string _id, char gen)
        {
            var passenger = _passenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();
            var restrictedPassenger = _restrictedPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();

            if (passenger != null)
            {
                passenger.Gender = gen;

                _passenger.ReplaceOne(p => p.CPF == _id, passenger);
                return passenger;
            }
            else if (restrictedPassenger != null)
            {

                passenger.Gender = gen;

                _restrictedPassenger.ReplaceOne(p => p.CPF == _id, passenger);
                return restrictedPassenger;
            }
            return null;
        }

        public Passenger UpdatePassengerPhone(string _id, string phone)
        {
            var passenger = _passenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();
            var restrictedPassenger = _restrictedPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();

            if (passenger != null)
            {
                passenger.Phone = phone;

                _passenger.ReplaceOne(p => p.CPF == _id, passenger);
                return passenger;
            }
            else if (restrictedPassenger != null)
            {

                passenger.Phone = phone;

                _restrictedPassenger.ReplaceOne(p => p.CPF == _id, passenger);
                return restrictedPassenger;
            }
            return null;
        }

        public Passenger UpdatePassengerBirthDate(string _id, DateTime birthdate)
        {
            var passenger = _passenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();
            var restrictedPassenger = _restrictedPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();

            if (passenger != null)
            {
                passenger.DtBirth = birthdate;

                _passenger.ReplaceOne(p => p.CPF == _id, passenger);
                return passenger;
            }
            else if (restrictedPassenger != null)
            {

                passenger.DtBirth = birthdate;

                _restrictedPassenger.ReplaceOne(p => p.CPF == _id, passenger);
                return restrictedPassenger;
            }
            return null;
        }

        public Passenger UpdatePassengerRegisterDate(string _id, DateTime registerDate)
        {
            var passenger = _passenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();
            var restrictedPassenger = _restrictedPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();

            if (passenger != null)
            {
                passenger.DtRegistry = registerDate;

                _passenger.ReplaceOne(p => p.CPF == _id, passenger);
                return passenger;
            }
            else if (restrictedPassenger != null)
            {

                passenger.DtRegistry = registerDate;

                _restrictedPassenger.ReplaceOne(p => p.CPF == _id, passenger);
                return restrictedPassenger;
            }
            return null;
        }

        public Passenger UpdatePassengerStatus(string _id, bool status)
        {
            var passenger = _passenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();
            var restrictedPassenger = _restrictedPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();

            if (passenger != null)
            {
                if (status == false && passenger.Status == true)
                {
                    passenger.Status = status;
                    _restrictedPassenger.InsertOne(passenger);
                    _passenger.DeleteOne(p => p.CPF == _id);
                }
                if (status == true && passenger.Status == false)
                {
                    passenger.Status = status;
                    _passenger.ReplaceOne(p => p.CPF == _id, passenger);
                }
                return passenger;
            }

            if (restrictedPassenger != null)
            {
                if (status == true && restrictedPassenger.Status == false)
                {
                    restrictedPassenger.Status = status;
                    _passenger.InsertOne(restrictedPassenger);
                    _restrictedPassenger.DeleteOne(p => p.CPF == _id);
                }
                if (status == false && restrictedPassenger.Status == true)
                {
                    restrictedPassenger.Status = status;
                    _restrictedPassenger.ReplaceOne(p => p.CPF == _id, restrictedPassenger);
                }
                return restrictedPassenger;
            }
            return null;
        }
        #endregion
        #region[D]
        public async Task<Passenger> Delete(string _id)
        {
            var passenger = _passenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();
            var restrictedPassenger = _restrictedPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();

            if(passenger != null)
            {
                await _unactivatedPassenger.InsertOneAsync(passenger);
                await _passenger.DeleteOneAsync(p => p.CPF == _id);
                return passenger;
            }

            if(restrictedPassenger != null)
            {
                await _unactivatedPassenger.InsertOneAsync(passenger);
                await _restrictedPassenger.DeleteOneAsync(p => p.CPF == _id);
                return restrictedPassenger;
            } 
            return new Passenger();
        }
        #endregion

        public async Task<Passenger> ReativatePassenger(string _id)
        {
            var unactivated = _unactivatedPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();

            if(unactivated != null && unactivated.Status == true)
            {
                await _passenger.InsertOneAsync(unactivated);
                await _unactivatedPassenger.DeleteOneAsync(p => p.CPF == _id);
                return unactivated;
            }
            if (unactivated != null && unactivated.Status == false)
            {
                await _restrictedPassenger.InsertOneAsync(unactivated);
                await _unactivatedPassenger.DeleteOneAsync(p => p.CPF == _id);
                return unactivated;
            }
            return new Passenger();
        }
        private int CalculateAge(DateTime bd)
        {
            var today = DateTime.Today;
            var age = today.Year - bd.Year;
            if (bd.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}

