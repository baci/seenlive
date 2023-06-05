using System.Collections.Generic;
using SeenLive.Core.DTOs;

namespace SeenLive.Web.Handler.DTOs
{
    public record ArtistCreationRequestDTO
    {
        public required string ArtistName { get; set; }

        public required IEnumerable<DateEntryCreationRequestDTO> DateEntryRequests { get; set; }
    }
}
