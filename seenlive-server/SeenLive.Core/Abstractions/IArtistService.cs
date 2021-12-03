using SeenLive.Core.Abstractions.Models;
using System.Collections.Generic;

namespace SeenLive.Core.Services
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
