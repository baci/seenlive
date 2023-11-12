using MongoDB.Driver;
using SeenLive.Core.Abstractions.Settings;
using System.Collections.Generic;
using SeenLive.Core.Abstractions;
using SeenLive.Core.Abstractions.Entities;
using SeenLive.DataAccess.Models;

namespace SeenLive.DataAccess.Services.MongoServices
{
    public class ArtistsRepository<T> : IArtistsRepository where T : IArtistEntity
    {
        private readonly IMongoCollection<T> _artistEntries;

        public ArtistsRepository(ISeenLiveDatabaseSettings settings, MongoDBContext context)
        {
            _artistEntries = context.Database.GetCollection<T>(settings.ArtistsCollectionName);
        }

        public IEnumerable<IArtistEntity> Get() =>
            _artistEntries.Find(entry => true).ToList().ConvertAll<IArtistEntity>(x => x);

        public IArtistEntity? Get(string id) =>
            _artistEntries.Find(entry => entry.Id == id).FirstOrDefault();

        public bool Update(string id, IArtistEntity newEntity) =>
            _artistEntries.ReplaceOne(entry => entry.Id == id, (T)newEntity).IsAcknowledged;

        public bool Remove(IArtistEntity oldEntity) =>
            _artistEntries.DeleteOne(entry => entry.Id == oldEntity.Id).IsAcknowledged;

        public bool Remove(string id) =>
            _artistEntries.DeleteOne(entry => entry.Id == id).IsAcknowledged;

        public IArtistEntity Create(string id, string artistName, IEnumerable<string> dateEntryIds)
        {
            IArtistEntity newEntity = new ArtistEntity(id, artistName, dateEntryIds);
            _artistEntries.InsertOne((T)newEntity);
            
            return newEntity;
        }
    }
}