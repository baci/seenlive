using MongoDB.Driver;
using SeenLive.Server.Settings;

namespace SeenLive.Server.Services.MongoServices
{
    public class MongoDBContext
    {
        public IMongoDatabase Database { get; }

        public MongoDBContext(ISeenLiveDatabaseSettings settings)
        {
            MongoClient client = new MongoClient(settings?.ConnectionString);
            Database = client.GetDatabase(settings.DatabaseName);
        }
    }
}
