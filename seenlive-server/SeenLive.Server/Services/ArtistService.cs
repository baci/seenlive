using MongoDB.Driver;
using SeenLive.Server.Models;
using System.Collections.Generic;
using System.Linq;

namespace SeenLive.Server.Services
{
    // TODO encapsulate mongo access. have a generic ArtistService that doesn't know about the DB implementation.
    public class ArtistService
    {
        private readonly IMongoCollection<ArtistEntry> _artistEntries;

        public ArtistService(ISeenLiveDatabaseSettings settings)
        {
            var client = new MongoClient(settings?.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _artistEntries = database.GetCollection<ArtistEntry>(settings.ArtistsCollectionName);
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

        public void Update(string id, ArtistEntry newEntry) =>
            _artistEntries.ReplaceOne(entry => entry.Id == id, newEntry);

        public void Remove(ArtistEntry oldEntry) =>
            _artistEntries.DeleteOne(entry => entry.Id == oldEntry.Id);

        public void Remove(string id) =>
            _artistEntries.DeleteOne(entry => entry.Id == id);
    }
}