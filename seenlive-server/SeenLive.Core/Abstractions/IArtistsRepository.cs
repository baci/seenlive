using System.Collections.Generic;
using SeenLive.Core.Abstractions.Entities;

namespace SeenLive.Core.Abstractions
{
    public interface IArtistsRepository
    {
        public IEnumerable<IArtistEntity> Get();

        public IArtistEntity? Get(string id);

        public IArtistEntity Create(string id, string artistName, IEnumerable<string> dateEntryIds);

        public bool Update(string id, IArtistEntity newEntity);

        public bool Remove(IArtistEntity oldEntity);

        public bool Remove(string id);
    }
}
