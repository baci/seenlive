using System.Collections.Generic;
using MongoDB.Driver;
using SeenLive.Core.Abstractions;
using SeenLive.Core.Abstractions.Entities;
using SeenLive.Core.Abstractions.Settings;
using SeenLive.DataAccess.Models;

namespace SeenLive.DataAccess.Services.MongoServices;

public class UserRepository<T> : IUserRepository where T : IUserEntity
{
    private readonly IMongoCollection<T> _userEntries;
    
    public UserRepository(ISeenLiveDatabaseSettings settings, MongoDBContext context)
    {
        _userEntries = context.Database.GetCollection<T>(settings.UsersCollectionName);
    }
    
    public IEnumerable<IUserEntity> Get() =>
        _userEntries.Find(entry => true).ToList().ConvertAll<IUserEntity>(x => x);

    public IUserEntity? Get(string id) => _userEntries.Find(entry => entry.Id == id).FirstOrDefault();

    public IUserEntity Create(string id, string userName)
    {
        IUserEntity newEntity = new UserEntity
        {
            Id = id,
            Username = userName
        };
        _userEntries.InsertOne((T)newEntity);
        
        return newEntity;
    }

    public bool Update(string id, IUserEntity newEntity) => _userEntries.ReplaceOne(entry => entry.Id == id, (T)newEntity).IsAcknowledged;

    public bool Remove(IUserEntity oldEntity) => _userEntries.DeleteOne(entry => entry.Id == oldEntity.Id).IsAcknowledged;

    public bool Remove(string id) => _userEntries.DeleteOne(entry => entry.Id == id).IsAcknowledged;
}