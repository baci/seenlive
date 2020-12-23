using SeenLive.DataAccess.Models;
using System.Collections.Generic;

namespace SeenLive.DataAccess.Services
{
    public interface IArtistService
    {
        public IEnumerable<ArtistEntry> Get();

        public ArtistEntry Get(string id);

        public ArtistEntry Create(ArtistEntry newEntry);

        public void Update(string id, ArtistEntry newEntry);

        public void Remove(ArtistEntry oldEntry);

        public void Remove(string id);
    }
}
