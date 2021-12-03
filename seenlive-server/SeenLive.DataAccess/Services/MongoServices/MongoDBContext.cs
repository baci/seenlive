using MongoDB.Driver;
using SeenLive.DataAccess.Settings.MongoSettings;

namespace SeenLive.DataAccess.Services.MongoServices
{
    public class MongoDBContext
    {
        public IMongoDatabase Database { get; }

        public MongoDBContext(SeenLiveDatabaseSettings settings)
        {
            MongoClient client = new MongoClient(settings?.ConnectionString);
            Database = client.GetDatabase(settings.DatabaseName);
        }
    }
}
