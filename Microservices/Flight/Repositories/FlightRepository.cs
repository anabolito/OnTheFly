﻿using System.Globalization;
using System.Web;
using FlightAPI.Utils;
using Models;
using Models.DTOs;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Services;

namespace FlightAPI.Repositories
{
    public class FlightRepository
    {
        private readonly IMongoCollection<Flight> _flights;
        private readonly IMongoCollection<Flight> _canceledFlights;
        private readonly IMongoCollection<Flight> _deletedFlights;
        private readonly AirportService _airportService;
        private readonly AircraftService _aircraftService;

        public FlightRepository(IFlightSettings settings, AirportService airportService, AircraftService aircraftService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var flightDatabase = client.GetDatabase(settings.DataBaseName);

            _flights = flightDatabase.GetCollection<Flight>(settings.FlightsCollectionName);
            _canceledFlights = flightDatabase.GetCollection<Flight>(settings.CanceledFlightsCollectionName);
            _deletedFlights = flightDatabase.GetCollection<Flight>(settings.DeletedFlightsCollectionName);
            _airportService = airportService;
            _aircraftService = aircraftService;
        }
        public async Task<List<Flight>> GetFlightAsync() =>
            _flights.FindAsync(f => true).Result.ToList();

        public async Task<List<Flight>> GetFlightAsync(string departure)
        {
            #region Filter
            var decodedDate = HttpUtility.UrlDecode(departure);
            var date = DateTime.ParseExact(decodedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var greater = Builders<Flight>.Filter.Gte(f => f.DtDeparture, date);
            var less = Builders<Flight>.Filter.Lt(f => f.DtDeparture, date.AddDays(1));

            var filter = Builders<Flight>.Filter.And(greater, less);
            #endregion

            return _flights.FindAsync(filter).Result.ToList();
        }

        public async Task<Flight> GetFlightAsync(string iata, string rab, string departure)
        {
            #region Filter
            var decodedDate = HttpUtility.UrlDecode(departure);
            var date = DateTime.ParseExact(decodedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var greater = Builders<Flight>.Filter.Gte(f => f.DtDeparture, date);
            var less = Builders<Flight>.Filter.Lt(f => f.DtDeparture, date.AddDays(1));

            var builder = Builders<Flight>.Filter;

            var airport = builder.Eq(f => f.Arrival.IATA, iata);
            var plane = builder.Eq(f => f.Plane.RAB, rab);

            var filter = builder.And(airport, plane, greater, less);
            #endregion

            return _flights.FindAsync(filter).Result.FirstOrDefault();
        }

        public async Task<Flight> PostFlightAsync(FlightDTO flightDTO)
        {
            var destinyAirport = await _airportService.GetIata(flightDTO.IataDestiny);
            var plane = _aircraftService.GetById(flightDTO.RabPlane).Result;

            if ((destinyAirport == null) || (plane == null))
                return null;

            if (!plane.Company.Status)
                return null;

            Airport destiny = new()
            {
                IATA = destinyAirport.iata,
                City = destinyAirport.city,
                State = destinyAirport.state,
                Country= destinyAirport.country_id
            };

            Flight flight = new()
            {
                Arrival = destiny,
                DtDeparture = flightDTO.DtDeparture,
                Plane = plane,
                Sales = 0,
                Status = flightDTO.Status,
            };

            var data = DateTime.ParseExact(flight.DtDeparture.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            flight.DtDeparture = data;

            if (flight.Status == true)
                await _flights.InsertOneAsync(flight);
            else
                await _canceledFlights.InsertOneAsync(flight);

            return flight;
        }

        public async Task<Flight> PutFlightAsync(string iata, string rab, string departure)
        {
            #region Filter
            var decodedDate = HttpUtility.UrlDecode(departure);
            var date = DateTime.ParseExact(decodedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var greater = Builders<Flight>.Filter.Gte(f => f.DtDeparture, date);
            var less = Builders<Flight>.Filter.Lt(f => f.DtDeparture, date.AddDays(1));

            var builder = Builders<Flight>.Filter;
            var airport = builder.Eq(f => f.Arrival.IATA, iata);
            var plane = builder.Eq(f => f.Plane.RAB, rab);

            var filter = builder.And(airport, plane, greater, less);
            #endregion

            var canceled = _flights.FindAsync(filter).Result.FirstOrDefault();
            canceled.Status = false;

            await _canceledFlights.InsertOneAsync(canceled);
            await _flights.DeleteOneAsync(filter);

            return canceled;
        }

        public async Task<Flight> PutFlightAsync(string iata, string rab, string departure, int count)
        {
            var decodedDate = HttpUtility.UrlDecode(departure);
            var date = DateTime.ParseExact(decodedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var greater = Builders<Flight>.Filter.Gte(f => f.DtDeparture, date);
            var less = Builders<Flight>.Filter.Lt(f => f.DtDeparture, date.AddDays(1));

            var builder = Builders<Flight>.Filter;
            var airport = builder.Eq(f => f.Arrival.IATA, iata);
            var plane = builder.Eq(f => f.Plane.RAB, rab);

            var filter = builder.And(airport, plane, greater, less);

            var flight = _flights.FindAsync(filter).Result.FirstOrDefault();

            if (flight != null)
            {
                flight.Sales += count;
                await _flights.ReplaceOneAsync(filter, flight);
            }

            return flight;
        }

        public async Task<bool> DeleteFlightAsync(string iata, string rab, string departure)
        {
            #region Filter
            var decodedDate = HttpUtility.UrlDecode(departure);
            var date = DateTime.ParseExact(decodedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var greater = Builders<Flight>.Filter.Gte(f => f.DtDeparture, date);
            var less = Builders<Flight>.Filter.Lt(f => f.DtDeparture, date.AddDays(1));

            var builder = Builders<Flight>.Filter;
            var airport = builder.Eq(f => f.Arrival.IATA, iata);
            var plane = builder.Eq(f => f.Plane.RAB, rab);

            var filter = builder.And(airport, plane, greater, less);
            #endregion
            Flight flight = _flights.FindAsync(filter).Result.FirstOrDefault();

            if (flight == null) 
                return false;

            await _deletedFlights.InsertOneAsync(flight);
            await _flights.DeleteOneAsync(filter);

            return true;
        }
        
    }
}
