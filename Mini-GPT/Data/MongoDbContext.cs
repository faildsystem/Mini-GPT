using Microsoft.Extensions.Options;
using Mini_GPT.Models;
using MongoDB.Driver;

namespace Mini_GPT.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Chat> _chatCollection;

        public MongoDbContext(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            _database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _chatCollection = _database.GetCollection<Chat>(mongoDbSettings.Value.ChatCollection);
        }

        public IMongoCollection<Chat> ChatCollection => _chatCollection;
    }
}