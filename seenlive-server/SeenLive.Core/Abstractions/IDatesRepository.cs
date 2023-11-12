using System.Collections.Generic;
using SeenLive.Core.Abstractions.Entities;

namespace SeenLive.Core.Abstractions
{
    public interface IDatesRepository
    {
        public IEnumerable<IDateEntity> Get();

        public IDateEntity Get(string id);

        public bool Update(string id, IDateEntity newEntity);

        public bool Remove(IDateEntity oldEntity);

        public bool Remove(string id);

        IDateEntity Create(string date, string? location, string? remarks);
    }
}
