using MongoDB.Driver;
using SeenLive.Core.Abstractions.Models;
using SeenLive.Core.Abstractions.Settings;
using System.Collections.Generic;
using System.Linq;
using SeenLive.Core.Abstractions;
using SeenLive.DataAccess.Models;

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

        public bool Update(string id, IArtistEntry newEntry) =>
            _artistEntries.ReplaceOne(entry => entry.Id == id, (T)newEntry).IsAcknowledged;

        public bool Remove(IArtistEntry oldEntry) =>
            _artistEntries.DeleteOne(entry => entry.Id == oldEntry.Id).IsAcknowledged;

        public bool Remove(string id) =>
            _artistEntries.DeleteOne(entry => entry.Id == id).IsAcknowledged;

        public IArtistEntry Create(string id, string artistName, IEnumerable<string> dateEntryIds)
        {
            IArtistEntry newEntry = new ArtistEntry(id, artistName, dateEntryIds);
            _artistEntries.InsertOne((T)newEntry);
            
            return newEntry;
        }
    }
}