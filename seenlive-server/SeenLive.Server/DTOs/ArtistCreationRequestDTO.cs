using System.Collections.Generic;

namespace SeenLive.Server.DTOs
{
    public class ArtistCreationRequestDTO
    {
        public string ArtistName { get; set; }

        public IEnumerable<DateEntryCreationRequestDTO> DateEntryRequests { get; set; }
    }
}
