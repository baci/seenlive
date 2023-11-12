using MongoDB.Driver;
using SeenLive.Core.Abstractions.Settings;
using System.Collections.Generic;
using SeenLive.Core.Abstractions;
using SeenLive.Core.Abstractions.Entities;
using SeenLive.DataAccess.Models;

namespace SeenLive.DataAccess.Services.MongoServices
{
    public class DatesRepository<T> : IDatesRepository where T : IDateEntity
    {
        private readonly IMongoCollection<T> _dateEntries;

        public DatesRepository(ISeenLiveDatabaseSettings settings, MongoDBContext context)
        {
            _dateEntries = context.Database.GetCollection<T>(settings.DatesCollectionName);
        }
        
        public IEnumerable<IDateEntity> Get() =>
            _dateEntries.Find(entry => true).ToList().ConvertAll<IDateEntity>(x => x);

        public IDateEntity Get(string id) =>
            _dateEntries.Find(entry => entry.Id == id).FirstOrDefault();

        public bool Update(string id, IDateEntity newEntity) =>
            _dateEntries.ReplaceOne(entry => entry.Id == id, (T)newEntity).IsAcknowledged;

        public bool Remove(IDateEntity oldEntity) =>
            _dateEntries.DeleteOne(entry => entry.Id == oldEntity.Id).IsAcknowledged;

        public bool Remove(string id) =>
            _dateEntries.DeleteOne(entry => entry.Id == id).IsAcknowledged;
        
        public IDateEntity Create(string date, string? location, string? remarks)
        {
            IDateEntity newEntity = new DateEntity { Date = date, Location = location, Remarks = remarks, Id = string.Empty };
            _dateEntries.InsertOne((T)newEntity);
            
            return newEntity;
        }
    }
}
