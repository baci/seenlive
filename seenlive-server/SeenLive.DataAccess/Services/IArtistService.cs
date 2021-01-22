using SeenLive.DataAccess.Models;
using System.Collections.Generic;

namespace SeenLive.DataAccess.Services
{
    public interface IArtistService
    {
        public IEnumerable<ArtistEntry> Get();

        public ArtistEntry Get(string id);

        public ArtistEntry Create(ArtistEntry newEntry);

        public bool Update(string id, ArtistEntry newEntry);

        public bool Remove(ArtistEntry oldEntry);

        public bool Remove(string id);
    }
}
