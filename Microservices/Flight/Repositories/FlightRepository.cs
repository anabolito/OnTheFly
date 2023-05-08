using System.Globalization;
using System.Web;
using FlightAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace FlightAPI.Repositories
{
    public class FlightRepository
    {
        private readonly IMongoCollection<Flight> _flights;
        private readonly IMongoCollection<Flight> _canceledFlights;

        public FlightRepository(IFlightSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var flightDatabase = client.GetDatabase(settings.DataBaseName);

            _flights = flightDatabase.GetCollection<Flight>(settings.FlightsCollectionName);
            _canceledFlights = flightDatabase.GetCollection<Flight>(settings.CanceledFlightsCollectionName);
        }

        #region Get
        public async Task<ActionResult<List<Flight>>> GetFlightAsync() =>
            _flights.FindAsync(f => true).Result.ToList();

        public async Task<ActionResult<List<Flight>>> GetFlightAsync(string departure)
        {

            var decodedDate = HttpUtility.UrlDecode(departure);
            var date = DateTime.ParseExact(decodedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var greater = Builders<Flight>.Filter.Gte(f => f.DtDeparture, date);
            var less = Builders<Flight>.Filter.Lt(f => f.DtDeparture, date.AddDays(1));

            var filter = Builders<Flight>.Filter.And(greater, less);

            return _flights.FindAsync(filter).Result.ToList();
        }
        #endregion

        #region Post
        public async Task<ActionResult<Flight>> PostFlightAsync(Flight flight)
        {
            if (flight == null)
                return new BadRequestResult();

            var data = DateTime.ParseExact(flight.DtDeparture.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            flight.DtDeparture = data;

            if (flight.Status == true)
                await _flights.InsertOneAsync(flight);
            else
                await _canceledFlights.InsertOneAsync(flight);

            return new OkResult();
        }
        #endregion

        #region Put
        public async Task<ActionResult> PutFlightAsync(Flight flight)
        {
            var builder = Builders<Flight>.Filter;

            var airport = builder.Eq(f => f.Destiny.IATA, flight.Destiny.IATA);
            var plane = builder.Eq(f => f.Plane.RAB, flight.Plane.RAB);
            var date = builder.Eq(f => f.DtDeparture, flight.DtDeparture);

            var filter = builder.And(airport, plane, date);

            var canceled = _flights.FindAsync(filter).Result.FirstOrDefault();
            canceled.Status = false;

            //await _flights.ReplaceOneAsync(filter, canceled);

            await _canceledFlights.InsertOneAsync(canceled);

            return new OkResult();
        }
        #endregion

        #region Delete
        public async Task<ActionResult<Flight>> DeleteFlightAsync(Flight flight)
        {
            var builder = Builders<Flight>.Filter;

            var airport = builder.Eq(f => f.Destiny.IATA, flight.Destiny.IATA);
            var plane = builder.Eq(f => f.Plane.RAB, flight.Plane.RAB);
            var date = builder.Eq(f => f.DtDeparture, flight.DtDeparture);

            var filter = builder.And(airport, plane, date);

            await _flights.DeleteOneAsync(filter);

            return new OkResult();
        }
        #endregion
    }
}
