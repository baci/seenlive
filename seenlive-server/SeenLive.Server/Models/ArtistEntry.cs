using System.Collections.Generic;

namespace SeenLive.Server.Models
{
    public class ArtistEntry
    {
        public string Id { get; set; }

        public string ArtistName { get; set; }

        public IList<DateEntry> DateEntries { get; }

        public ArtistEntry(string id, string artistName)
        {
            DateEntries = new List<DateEntry>();
            Id = id;
            ArtistName = artistName;
        }
    }
}
