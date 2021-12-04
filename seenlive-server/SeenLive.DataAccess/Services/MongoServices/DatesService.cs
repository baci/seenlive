using MongoDB.Driver;
using SeenLive.Core.Abstractions.Models;
using SeenLive.Core.Abstractions.Settings;
using System.Collections.Generic;
using SeenLive.Core.Abstractions;

namespace SeenLive.DataAccess.Services.MongoServices
{
    public class DatesService<T> : IDatesService where T : IDateEntry
    {
        private readonly IMongoCollection<T> _dateEntries;

        public DatesService(ISeenLiveDatabaseSettings settings, MongoDBContext context)
        {
            _dateEntries = context.Database.GetCollection<T>(settings.DatesCollectionName);
        }

        public IEnumerable<IDateEntry> Get() =>
            _dateEntries.Find(entry => true).ToList().ConvertAll<IDateEntry>(x => x);

        public IDateEntry Get(string id) =>
            _dateEntries.Find(entry => entry.Id == id).FirstOrDefault();

        public IDateEntry Create(IDateEntry newEntry)
        {
            _dateEntries.InsertOne((T)newEntry);
            return newEntry;
        }

        public bool Update(string id, IDateEntry newEntry) =>
            _dateEntries.ReplaceOne(entry => entry.Id == id, (T)newEntry).IsAcknowledged;

        public bool Remove(IDateEntry oldEntry) =>
            _dateEntries.DeleteOne(entry => entry.Id == oldEntry.Id).IsAcknowledged;

        public bool Remove(string id) =>
            _dateEntries.DeleteOne(entry => entry.Id == id).IsAcknowledged;
    }
}
