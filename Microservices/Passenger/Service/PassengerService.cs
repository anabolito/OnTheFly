using PassengerAPI.Utils;
using Models;
using MongoDB.Driver;

namespace PassengerAPI.Repositories
{
    public class PassengerService
    {
        private readonly IMongoCollection<Passenger> _passenger;

        public PassengerService(IPassengerSettings settings)
        {
            var passenger = new MongoClient(settings.ConnectionString);
            var database = passenger.GetDatabase(settings.DatabaseName);
            _passenger = database.GetCollection<Passenger>(settings.PassengerCollectionName);
        }

        #region[C]
        public Passenger Create (Passenger passenger)
        {
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
            var today = DateTime.Today;
            var eighteenYearsAgo = today.AddYears(-18);
            var underAgeOnes = _passenger.Find (p => p.DtBirth > DateOnly.FromDateTime(eighteenYearsAgo)).ToList();
            return underAgeOnes;
        }

        public List<Passenger> GetRestricted() 
        { 
            var restrictedOnes = _passenger.Find(p => p.Status == false).ToList();
            return restrictedOnes;
        }

        public Passenger GetByCPF(string cpf)
        {
            if(string.IsNullOrEmpty(cpf)) return new Passenger();
            return _passenger.Find<Passenger>(x => x.CPF == cpf).FirstOrDefault();
        }
        #endregion
        #region[U]
        public Passenger Update(string cpf, Passenger passenger)
        {            
            _passenger.ReplaceOne(p => p.CPF == cpf, passenger);
            return passenger;
        }
        #endregion
        #region[D]
        public List<Passenger> Delete(string cpf) 
        {
            var passengerToDelete = _passenger.Find<Passenger>(passenger => passenger.CPF == cpf).FirstOrDefault();
            List<Passenger> deletedOnes = new();
            deletedOnes.Append(passengerToDelete);
            _passenger.DeleteOne(p => p.CPF == cpf);
            return deletedOnes;
        }
        #endregion

        //#region[ValidateDoc]
        //public bool ValidateDoc(string cpf)
        //{
        //    int[] numbers = new int[11];
        //    int sumA = 0, sumB = 0;
        //    double restA = 0, restB = 0;

        //    cpf = cpf.Replace(".", "").Replace("-", "");

        //    if (cpf.Length < 11) return false;

        //    for (int i = 0; i < 11; i++)
        //    {
        //        if (!int.TryParse(cpf[i].ToString(),out numbers[i]))                
        //            return false;                
        //    }

        //    if (numbers[0] == numbers[1] && numbers[1] == numbers[2] && numbers[2] == numbers[3] && numbers[3] == numbers[4] &&
        //        numbers[4] == numbers[5] && numbers[5] == numbers[6] && numbers[6] == numbers[7] && numbers[7] == numbers[8] &&
        //        numbers[9] == numbers[9] && numbers[10] == numbers[10]) return false;

        //    for (int i = 0, j = 10; i < 9; i++, j--)
        //    {
        //        sumA += numbers[i] * j;
        //    }

        //    restA = sumA % 11;

        //    if (restA < 2)           
        //        restA = 0;            
        //    else            
        //        restA = 11 - restA;
            

        //    for (int i = 0, j = 11; i < 10; i++, j--)
        //    {
        //        sumB += numbers[i] * j;
        //    }

        //    restB = sumB % 11;

        //    if(restB < 2)            
        //        restB = 0;            
        //    else            
        //        restB += 11 - restB;

        //    if(restA  == numbers[9] && restB == numbers[10])            
        //        return true;            
        //    else            
        //        return false;            
        //}
        //#endregion
    }
}

