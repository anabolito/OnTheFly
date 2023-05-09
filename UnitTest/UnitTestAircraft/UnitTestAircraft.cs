using MongoDB.Driver;
using NUnit.Framework;
using MongoDB.Bson;

namespace UnitTestAircraft
{
    [TestFixture]
    public class UnitTestAircraft
    {
        private IMongoDatabase _database;

        [SetUp]
        public void SetUp()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            _database = client.GetDatabase("aeronavesDB");
        }
        /*
        [TearDown]
        public async Task TearDownAsync()
        {
            await _database.DropCollectionAsync("aeronaves");
        }
        */

        [Test]
        public async Task Test_Insert()
        {
            // Arrange
            var collection = _database.GetCollection<BsonDocument>("aeronaves");
            var document = new BsonDocument { { "RAB", "Capacity" } };

            // Act
            await collection.InsertOneAsync(document);

            // Assert
            var result = await collection.Find(document).FirstOrDefaultAsync();
            NUnit.Framework.Assert.AreEqual(document, result);
        }
    }
}