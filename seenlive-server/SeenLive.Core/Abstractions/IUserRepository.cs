using System.Collections.Generic;
using SeenLive.Core.Abstractions.Entities;

namespace SeenLive.Core.Abstractions;

public interface IUserRepository
{
    public IEnumerable<IUserEntity> Get();

    public IUserEntity? Get(string id);

    public IUserEntity Create(string id, string userName);

    public bool Update(string id, IUserEntity newEntity);

    public bool Remove(IUserEntity oldEntity);

    public bool Remove(string id);
}