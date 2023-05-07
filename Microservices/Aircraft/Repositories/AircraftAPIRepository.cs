﻿using AircraftAPI.Config;
using Models;
using MongoDB.Driver;

namespace AircraftAPI.Services
{
    public class AircraftAPIRepository
    {

        private readonly IMongoCollection<Aircraft> _aircraft;

        public AircraftAPIRepository(IAircraftAPISettings settings)
        {
            var aircraft = new MongoClient(settings.ConnectionString);
            var database = aircraft.GetDatabase(settings.DatabaseName);
            _aircraft = database.GetCollection<Aircraft>(settings.AircraftCollectionName);
        }

        public List<Aircraft> Get()
        {
            return _aircraft.Find(c => true).ToList();
        }

        public Aircraft Get(string id)
        {
            return _aircraft.Find<Aircraft>(c => c.RAB == id).FirstOrDefault();
        }

        public Aircraft Create(Aircraft aircraft)
        {
            _aircraft.InsertOne(aircraft);
            return aircraft;
        }

        public void Update(Aircraft aircraft)
        {
            _aircraft.ReplaceOne(c => c.RAB == aircraft.RAB, aircraft);
        }

        public void Delete(string id)
        {
            _aircraft.DeleteOne(c => c.RAB == id);
        }

    }
}
