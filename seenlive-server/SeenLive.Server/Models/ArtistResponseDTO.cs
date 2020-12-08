using System.Collections.Generic;
using System.Linq;

namespace SeenLive.Server.Models
{
    public class ArtistResponseDTO
    {
        public string Id { get; set; }

        public string ArtistName { get; }

        public IList<DateEntry> DateEntries { get; private set; }

        public ArtistResponseDTO(string id, string artistName, IList<DateEntry> dateEntries)
        {
            DateEntries = dateEntries;
            Id = id;
            ArtistName = artistName;
        }
    }
}
