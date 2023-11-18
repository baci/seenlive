using System.Collections.Generic;

namespace SeenLive.Web.Handler.DTOs
{
    public record ArtistCreationRequestDTO
    {
        public required string UserId { get; set; }
        
        public required string ArtistName { get; set; }

        public required List<DateEntryCreationRequestDTO> DateEntryRequests { get; set; }
    }
}
