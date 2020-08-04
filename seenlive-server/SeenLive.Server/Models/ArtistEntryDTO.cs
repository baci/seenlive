using System.Collections.Generic;

namespace SeenLive.Server.Models
{
    public class ArtistEntryDTO
    {
        public string Id { get; set; }

        public string Artist { get; set; }

        public IList<DateEntryDTO> DateEntries { get; set; }
    }
}
