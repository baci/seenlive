using MongoDB.Driver;
using SeenLive.DataAccess.Models;
using SeenLive.DataAccess.Settings;
using System.Collections.Generic;
using System.Linq;

namespace SeenLive.DataAccess.Services.MongoServices
{
    public class ArtistService : IArtistService
    {
        private readonly IMongoCollection<ArtistEntry> _artistEntries;

        public ArtistService(ISeenLiveDatabaseSettings settings, MongoDBContext context)
        {
            _artistEntries = context.Database.GetCollection<ArtistEntry>(settings.ArtistsCollectionName);
        }

        public IEnumerable<ArtistEntry> Get() =>
            _artistEntries.Find(entry => true).ToList();

        public ArtistEntry Get(string id) =>
            _artistEntries.Find(entry => entry.Id == id).FirstOrDefault();

        public ArtistEntry Create(ArtistEntry newEntry)
        {
            _artistEntries.InsertOne(newEntry);
            return newEntry;
        }

        public bool Update(string id, ArtistEntry newEntry) =>
            _artistEntries.ReplaceOne(entry => entry.Id == id, newEntry).IsAcknowledged;

        public bool Remove(ArtistEntry oldEntry) =>
            _artistEntries.DeleteOne(entry => entry.Id == oldEntry.Id).IsAcknowledged;

        public bool Remove(string id) =>
            _artistEntries.DeleteOne(entry => entry.Id == id).IsAcknowledged;
    }
}