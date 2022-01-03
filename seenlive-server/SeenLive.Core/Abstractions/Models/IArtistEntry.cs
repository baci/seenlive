using System.Collections.Generic;

namespace SeenLive.Core.Abstractions.Models
{
    public interface IArtistEntry
    {
        public string Id { get; set; }

        public string ArtistName { get; set; }

        public IList<string> DateEntryIDs { get; }

        void AddDateEntries(IEnumerable<string> dateEntryIDs);
    }
}
