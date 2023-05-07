using FlightAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Models;
using MongoDB.Bson;
using MongoDB.Driver;

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
        public ActionResult<List<Flight>> GetFlight() => _flights.Find(f => true).ToList();

        public ActionResult<List<Flight>> GetFlight(DateOnly departure) => _flights.Find(f => f.DtDeparture.Equals(departure)).ToList();
        #endregion

        #region Post
        public ActionResult<Flight> PostFlight(Flight flight)
        {
            _flights.InsertOne(flight);

            return new OkResult();
        }
        #endregion

        #region Put
        public ActionResult<Flight> PutFlight(Flight flight)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Delete
        public ActionResult<Flight> DeleteFlight(Flight flight)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
