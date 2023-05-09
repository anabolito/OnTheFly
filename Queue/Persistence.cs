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

        public void InsertSale(byte[] sale)
        {
            var saleSerialized = BsonSerializer.Deserialize<BsonDocument>(sale);
            var saleDeserialized = BsonSerializer.Deserialize<Sale>(sale);
            if (saleDeserialized.Sold)
            {
                collectionSales.InsertOne(saleSerialized);
            }
            else
                collectionReservedSales.InsertOne(saleSerialized);
        }
    }
}