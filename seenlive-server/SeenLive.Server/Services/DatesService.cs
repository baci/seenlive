using MongoDB.Driver;
using SeenLive.Server.Models;
using System.Collections.Generic;

namespace SeenLive.Server.Services
{
    public class DatesService
    {
        private readonly IMongoCollection<DateEntry> _dateEntries;

        public DatesService(ISeenLiveDatabaseSettings settings)
        {
            // TODO encapsulate a single MongoClient somewhere for all services
            var client = new MongoClient(settings?.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _dateEntries = database.GetCollection<DateEntry>(settings.DatesCollectionName);
        }

        public IEnumerable<DateEntry> Get() =>
            _dateEntries.Find(entry => true).ToList();

        public DateEntry Get(string id) =>
            _dateEntries.Find(entry => entry.Id == id).FirstOrDefault();

        public DateEntry Create(DateEntry newEntry)
        {
            _dateEntries.InsertOne(newEntry);
            return newEntry;
        }

        public void Update(string id, DateEntry newEntry) =>
            _dateEntries.ReplaceOne(entry => entry.Id == id, newEntry);

        public void Remove(DateEntry oldEntry) =>
            _dateEntries.DeleteOne(entry => entry.Id == oldEntry.Id);

        public void Remove(string id) =>
            _dateEntries.DeleteOne(entry => entry.Id == id);
    }
}
