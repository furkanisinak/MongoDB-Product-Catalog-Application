using MongoDB.Driver;
using MongoDB.Bson;

namespace Program.Utils
{
    public class DBMongo
    {
        private readonly string connectionString = "mongodb://localhost:27017";
        private static readonly string databaseName = "admin";
        private readonly IMongoDatabase _database;

        public DBMongo()
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}