using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Models;
using MongoDB.Driver;
using RabbitMQ.Client;
using SaleAPI.Utils;
using Models.DTOs;
using System.Globalization;
using System.Web;
using Services;

namespace SaleAPI.Repository
{
    public class SaleRepository
    {
        private readonly IMongoCollection<Sale> _sales;
        private readonly IMongoCollection<Sale> _reservedSales;
        private readonly IMongoCollection<Sale> _deletedSales;
        private readonly ConnectionFactory _factory;
        private readonly FlightService _flightService;
        private readonly PassengerService _passengerService;
        private string QUEUE_NAME;


        public SaleRepository(ISaleSettings settings, ConnectionFactory factory, FlightService flightService, PassengerService passengerService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var flightDatabase = client.GetDatabase(settings.DataBaseName);

            _sales = flightDatabase.GetCollection<Sale>(settings.SalesCollectionName);
            _reservedSales = flightDatabase.GetCollection<Sale>(settings.ReservedSalesCollectionName);
            _deletedSales = flightDatabase.GetCollection<Sale>(settings.DeletedSalesCollectionName);
            _factory = factory;
            _flightService = flightService;
            _passengerService = passengerService;
        }


        #region Get
        public async Task<ActionResult<List<Sale>>> GetSalesAsync()
        {
            List<Sale> sales = new();
            var saleList = _sales.FindAsync(f => true).Result.ToList();
            var reservedList = _reservedSales.FindAsync(f => true).Result.ToList();

            sales.AddRange(saleList);
            sales.AddRange(reservedList);

            return sales;
        }
        #endregion

        #region Post
        public async Task<Sale> PostSalesAsync([FromBody] SaleDTO saleDTO)
        {

            var flight = _flightService.Get(saleDTO.IataFlight, saleDTO.RabFlight, saleDTO.DtDepartureFlight).Result;

            List<Passenger> passengerList = new();
            foreach(var item in saleDTO.Cpf)
            {
                passengerList.Add(_passengerService.GetByCPF(item).Result);
            }


            //saleDTO.Cpf.ForEach(async c =>  passengerList.Add(_passengerService.GetByCPF(c).Result));

            Sale sale = new()
            {
                Id = null,
                Flight = flight,
                Passengers = passengerList,
                Reserved = saleDTO.Reserved,
                Sold = saleDTO.Sold
            };

            //int date = CalcularIdade(sale.Passengers[0].DtBirth);

            //if (date < 18)
            //    return false;

            //foreach (var item in sale.Passengers)
            //{
            //    if (item.Status == false)
            //        return false;
            //}

            if (sale.Passengers.GroupBy(x => x).Any(p => p.Count() > 1))
                return null;

            if (sale.Sold == sale.Reserved)
                return null;

            if ((sale.Sold == true) && (sale.Reserved == false))
                QUEUE_NAME = "sold";
            else
                QUEUE_NAME = "reserved";

            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {

                    channel.QueueDeclare(
                        queue: QUEUE_NAME,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );

                    var stringfieldMessage = JsonConvert.SerializeObject(sale);
                    var bytesMessage = Encoding.UTF8.GetBytes(stringfieldMessage);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: QUEUE_NAME,
                        basicProperties: null,
                        body: bytesMessage
                        );
                }
            }
            return sale;
        }
        #endregion

        #region Put
        public async Task<ActionResult> PutSalesAsync(string cpf, string departure)
        {
            #region Filter
            var decodedDate = HttpUtility.UrlDecode(departure);
            var date = DateTime.ParseExact(decodedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var greater = Builders<Sale>.Filter.Gte(s => s.Flight.DtDeparture, date);
            var less = Builders<Sale>.Filter.Lt(s => s.Flight.DtDeparture, date.AddDays(1));

            var builder = Builders<Sale>.Filter;
            var passenger = builder.Eq(s => s.Passengers[0].CPF, cpf);

            var filter = builder.And(passenger, greater, less);
            #endregion

            Sale saleFindedReserved = _reservedSales.Find(filter).FirstOrDefault();
            saleFindedReserved.Reserved = false;
            saleFindedReserved.Sold = true;

            if (saleFindedReserved.Sold)
            {
                _reservedSales.DeleteOne(filter);
                await _sales.InsertOneAsync(saleFindedReserved);
            }

            return new OkResult();
        }
        #endregion

        #region Delete
        public async Task<ActionResult> DeleteSalesAsync(string cpf, string departure)
        {
            #region Filter
            var decodedDate = HttpUtility.UrlDecode(departure);
            var date = DateTime.ParseExact(decodedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var greater = Builders<Sale>.Filter.Gte(s => s.Flight.DtDeparture, date);
            var less = Builders<Sale>.Filter.Lt(s => s.Flight.DtDeparture, date.AddDays(1));

            var builder = Builders<Sale>.Filter;
            var passenger = builder.Eq(s => s.Passengers[0].CPF, cpf);

            var filter = builder.And(passenger, greater, less);
            #endregion

            Sale saleFindedReserved = _reservedSales.Find(filter).FirstOrDefault();
            Sale saleFinded = _sales.Find(filter).FirstOrDefault();


            if (saleFindedReserved.Reserved)
            {
                await _deletedSales.InsertOneAsync(saleFindedReserved);
                await _reservedSales.DeleteOneAsync(filter);
            }
            else
            {
                await _deletedSales.InsertOneAsync(saleFinded);
                await _sales.DeleteOneAsync(filter);
            }


            return new OkResult();
        }
        #endregion

        private int CalcularIdade(DateTime dataNascimento)
        {
            int idade = DateTime.Today.Year - dataNascimento.Year;

            if (DateTime.Today.Month < dataNascimento.Month || (DateTime.Today.Month == dataNascimento.Month && DateTime.Today.Day < dataNascimento.Day))
            {
                idade--;
            }

            return idade;
        }
    }
}
