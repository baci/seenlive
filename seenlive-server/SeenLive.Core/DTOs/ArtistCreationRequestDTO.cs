using System.Collections.Generic;

namespace SeenLive.Core.DTOs
{
    public class ArtistCreationRequestDTO
    {
        public string ArtistName { get; set; }

        public IEnumerable<DateEntryCreationRequestDTO> DateEntryRequests { get; set; }
    }
}
