using PassengerAPI.Utils;
using Models;
using MongoDB.Driver;
using PassengerAPI.DTO;
using PassengerAPI.AddressService;
using System.Security.Cryptography;
using System.Web;
using System.Globalization;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace PassengerAPI.Repositories
{
    public class PassengerRepository
    {
        private readonly IMongoCollection<Passenger> _customPassenger;
        private readonly IMongoCollection<Passenger> _unactivatedPassenger;
        private readonly IMongoCollection<Passenger> _restrictedPassenger;

        public PassengerRepository(IPassengerSettings settings)
        {
            var passenger = new MongoClient(settings.ConnectionString);
            var database = passenger.GetDatabase(settings.DatabaseName);

            _customPassenger = database.GetCollection<Passenger>(settings.PassengerCollectionName);
            _unactivatedPassenger = database.GetCollection<Passenger>(settings.InactivePassengerCollectionName);
            _restrictedPassenger = database.GetCollection<Passenger>(settings.RestrictPassengerCollectionName);
        }

        #region[C]
        public Passenger Create(Passenger passenger)
        {
            _customPassenger.InsertOne(passenger);

            return passenger;
        }
        #endregion
        #region[R]
        public List<Passenger> Get()
        {
            var allPassengers = new List<Passenger>();
            var normalPassengers = _customPassenger.Find(p => true).ToList();
            var restrictedPassengers = _restrictedPassenger.Find(p => true).ToList();

            if (normalPassengers.Count != 0) allPassengers.AddRange(normalPassengers);

            if (restrictedPassengers.Count != 0) allPassengers.AddRange(restrictedPassengers);

            if (allPassengers.Count != 0) return allPassengers;

            return null;
            //Console.WriteLine("Não existem passageiros cadastrados!");
        }

        public List<Passenger> GetCustomPassenger()
        {
            var passenger = _customPassenger.Find(u => true).ToList();

            if (passenger == null)
            {
                Console.WriteLine("Passageiro não encontrado!");
                throw new BadHttpRequestException("Passageiro não encontrado!");
            }

            return passenger;
        }

        public List<Passenger> GetAllMinors()
        {
            var underAgePassengers = new List<Passenger>();
            var passengers = _customPassenger.Find(p => true).ToList();
            var restrictedPassengers = _restrictedPassenger.Find(p => true).ToList();

            var minors = passengers.Where(p => CalculateAge(p.DtBirth) < 18).ToList();
            var restrictedMinors = restrictedPassengers.Where(p => CalculateAge(p.DtBirth) < 18).ToList();

            underAgePassengers.AddRange(minors);
            underAgePassengers.AddRange(restrictedMinors);

            if (underAgePassengers.Count == 0)
            {
                Console.WriteLine("Não existem passageiros menores de idade!");
                return null;
            }

            return underAgePassengers;
        }

        public List<Passenger> GetRestrictedOnes()
        {
            var restrictedOnes = _restrictedPassenger.Find(r => true).ToList();

            if (restrictedOnes.Count == 0)
            {
                Console.WriteLine("Não existem passageiros restritos!");
                return null;
            }

            return restrictedOnes;
        }

        public List<Passenger> GetDeletedOnes()
        {
            var deletedOnes = _unactivatedPassenger.Find(u => true).ToList();

            if (deletedOnes.Count == 0)
            {
                Console.WriteLine("Não existem passageiros desativados!");
                return null;
            }

            return deletedOnes;
        }

        public Passenger GetByCPF(string _id)
        {
            var customPassenger = _customPassenger.Find(passenger => passenger.CPF == _id).FirstOrDefault();
            var restrictedPassenger = _restrictedPassenger.Find(passenger => passenger.CPF == _id).FirstOrDefault();

            if (customPassenger != null) return customPassenger;
            else if (restrictedPassenger != null) return restrictedPassenger;

            return null;

        }

        #endregion
        #region[U]
        public Passenger UpdatePassengerAddress(string _id, string cep, int number, string complement)
        {
            var passenger = _customPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();
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
                _customPassenger.ReplaceOne(p => p.CPF == _id, passenger);
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

        public Passenger UpdatePassengerAddressStreet(string _id, string streetName)
        {
            var passenger = _customPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();
            var restrictedPassenger = _restrictedPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();


            if (passenger != null)
            {
                var currentAddress = passenger.Address;

                if (currentAddress != null)
                {
                    currentAddress.Street = streetName;
                }
                currentAddress.Street = streetName;
                passenger.Address = currentAddress;

                _customPassenger.ReplaceOne(p => p.CPF == _id, passenger);
                return passenger;
            }
            else if (restrictedPassenger != null)
            {
                var currentAddress = passenger.Address;

                if (currentAddress != null)
                {
                    currentAddress.Street = streetName;
                }
                currentAddress.Street = streetName;
                restrictedPassenger.Address = currentAddress;

                _restrictedPassenger.ReplaceOne(p => p.CPF == _id, restrictedPassenger);
                return restrictedPassenger;
            }
            return null;
        }

        public Passenger UpdatePassengerName(string _id, string name)
        {
            var passenger = _customPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();
            var restrictedPassenger = _restrictedPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();

            if (passenger != null)
            {
                passenger.Name = name;

                _customPassenger.ReplaceOne(p => p.CPF == _id, passenger);
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
            var passenger = _customPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();
            var restrictedPassenger = _restrictedPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();

            if (passenger != null)
            {
                passenger.Gender = gen;

                _customPassenger.ReplaceOne(p => p.CPF == _id, passenger);
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
            var passenger = _customPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();
            var restrictedPassenger = _restrictedPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();

            if (passenger != null)
            {
                passenger.Phone = phone;

                _customPassenger.ReplaceOne(p => p.CPF == _id, passenger);
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

        public Passenger UpdatePassengerStatus(string _id)
        {
            var passenger = _customPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();
            var restrictedPassenger = _restrictedPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();

            if (passenger != null)
            {
                if (passenger.Status == true) passenger.Status = false;
                else if (passenger.Status == false) passenger.Status = true;
                _customPassenger.ReplaceOne(p => p.CPF == _id, passenger);
                return passenger;
            }

            if (restrictedPassenger != null)
            {
                if (restrictedPassenger.Status == true) restrictedPassenger.Status = false;
                else if (restrictedPassenger.Status == false) restrictedPassenger.Status = true ;
                _restrictedPassenger.ReplaceOne(p => p.CPF == _id, passenger);
                return restrictedPassenger;
            }
            return null;
        }

        public Passenger SetPassengerAsRestricted(string _id)
        {
            var passenger = _customPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();
            if (passenger != null)
            {
                _restrictedPassenger.InsertOne(passenger);
                _customPassenger.DeleteOne(x => x.CPF == _id);
                return passenger;
            }
            return null;
        }

        public Passenger SetPassengerAsUnrestricted(string _id)
        {
            var restrictedPassenger = _restrictedPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();
            if (restrictedPassenger != null)
            {
                _customPassenger.InsertOne(restrictedPassenger);
                _restrictedPassenger.DeleteOne(x => x.CPF == _id);
                return restrictedPassenger;
            }
            return null;
        }

        #endregion
        #region[D]
        public async Task<Passenger> Delete(string _id)
        {
            var passenger = _customPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();
            var restrictedPassenger = _restrictedPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();

            if (passenger != null)
            {
                await _unactivatedPassenger.InsertOneAsync(passenger);
                await _customPassenger.DeleteOneAsync(p => p.CPF == _id);
                return passenger;
            }

            if (restrictedPassenger != null)
            {
                await _unactivatedPassenger.InsertOneAsync(passenger);
                await _restrictedPassenger.DeleteOneAsync(p => p.CPF == _id);
                return restrictedPassenger;
            }
            return null;
        }
        #endregion

        public async Task<Passenger> ReativatePassenger(string _id)
        {
            var unactivated = _unactivatedPassenger.Find<Passenger>(x => x.CPF == _id).FirstOrDefault();

            if (unactivated != null && unactivated.Status == true)
            {
                await _customPassenger.InsertOneAsync(unactivated);
                await _unactivatedPassenger.DeleteOneAsync(p => p.CPF == _id);
                return unactivated;
            }
            if (unactivated != null && unactivated.Status == false)
            {
                await _restrictedPassenger.InsertOneAsync(unactivated);
                await _unactivatedPassenger.DeleteOneAsync(p => p.CPF == _id);
                return unactivated;
            }
            return null;
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

