﻿using MongoDB.Driver;
using SeenLive.DataAccess.Models;
using SeenLive.DataAccess.Settings;
using System.Collections.Generic;

namespace SeenLive.DataAccess.Services.MongoServices
{
    public class DatesService : IDatesService
    {
        private readonly IMongoCollection<DateEntry> _dateEntries;

        public DatesService(ISeenLiveDatabaseSettings settings, MongoDBContext context)
        {
            _dateEntries = context.Database.GetCollection<DateEntry>(settings.DatesCollectionName);
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

        public bool Update(string id, DateEntry newEntry) =>
            _dateEntries.ReplaceOne(entry => entry.Id == id, newEntry).IsAcknowledged;

        public bool Remove(DateEntry oldEntry) =>
            _dateEntries.DeleteOne(entry => entry.Id == oldEntry.Id).IsAcknowledged;

        public bool Remove(string id) =>
            _dateEntries.DeleteOne(entry => entry.Id == id).IsAcknowledged;
    }
}
