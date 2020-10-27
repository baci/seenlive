using System.Collections.Generic;

namespace SeenLive.Server.Models
{
    public class ArtistCreationRequestDTO
    {
        public string ArtistName { get; set; }

        public IEnumerable<DateEntryCreationRequestDTO> DateEntryRequests { get; set; }
    }
}
