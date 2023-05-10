using System.Text.Json;
using Models;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace QueuePersistence
{
    public class Persistence
    {
        string _connectionString = "mongodb://localhost:27017";
        string _databaseName = "Sales";
        string _salesCollectionName = "Sales";
        string _reservedSalesCollectionName = "ReservedSales";

        IMongoCollection<BsonDocument> collectionSales;
        IMongoCollection<BsonDocument> collectionReservedSales;

        public Persistence()
        {
            MongoClient client = new MongoClient(_connectionString);

            var database = client.GetDatabase(_databaseName);
            collectionSales = database.GetCollection<BsonDocument>(_salesCollectionName);
            collectionReservedSales = database.GetCollection<BsonDocument>(_reservedSalesCollectionName);
        }

        public void InsertSale(BsonDocument bson, Sale sale)
        {
            if (sale.Sold)
            {
                collectionSales.InsertOne(bson);
            }
            else
                collectionReservedSales.InsertOne(bson);
        }
    }
}