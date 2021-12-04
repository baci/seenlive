using System.Collections.Generic;
using SeenLive.Core.Abstractions.Models;

namespace SeenLive.Core.Abstractions
{
    public interface IArtistService
    {
        public IEnumerable<IArtistEntry> Get();

        public IArtistEntry Get(string id);

        public IArtistEntry Create(IArtistEntry newEntry);

        public bool Update(string id, IArtistEntry newEntry);

        public bool Remove(IArtistEntry oldEntry);

        public bool Remove(string id);
    }
}
