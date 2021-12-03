using MongoDB.Driver;
using SeenLive.Core.Abstractions.Models;
using SeenLive.Core.Abstractions.Settings;
using SeenLive.Core.Services;
using System.Collections.Generic;
using System.Linq;

namespace SeenLive.DataAccess.Services.MongoServices
{
    public class ArtistService<T> : IArtistService where T : IArtistEntry
    {
        private readonly IMongoCollection<T> _artistEntries;

        public ArtistService(ISeenLiveDatabaseSettings settings, MongoDBContext context)
        {
            _artistEntries = context.Database.GetCollection<T>(settings.ArtistsCollectionName);
        }

        public IEnumerable<IArtistEntry> Get() =>
            _artistEntries.Find(entry => true).ToList().ConvertAll<IArtistEntry>(x => x);

        public IArtistEntry Get(string id) =>
            _artistEntries.Find(entry => entry.Id == id).FirstOrDefault();

        public IArtistEntry Create(IArtistEntry newEntry)
        {
            _artistEntries.InsertOne((T)newEntry);
            return newEntry;
        }

        public bool Update(string id, IArtistEntry newEntry) =>
            _artistEntries.ReplaceOne(entry => entry.Id == id, (T)newEntry).IsAcknowledged;

        public bool Remove(IArtistEntry oldEntry) =>
            _artistEntries.DeleteOne(entry => entry.Id == oldEntry.Id).IsAcknowledged;

        public bool Remove(string id) =>
            _artistEntries.DeleteOne(entry => entry.Id == id).IsAcknowledged;
    }
}