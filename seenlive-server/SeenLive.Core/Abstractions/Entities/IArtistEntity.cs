using System.Collections.Generic;

namespace SeenLive.Core.Abstractions.Entities
{
    public interface IArtistEntity
    {
        public string Id { get; set; }

        public string ArtistName { get; set; }

        public IList<string> DateEntryIDs { get; set; }

        void AddDateEntries(IEnumerable<string> dateEntryIDs);
    }
}
