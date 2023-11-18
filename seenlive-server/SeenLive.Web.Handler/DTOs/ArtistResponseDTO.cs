using System.Collections.Generic;

namespace SeenLive.Web.Handler.DTOs
{
    public record ArtistResponseDTO
    {
        public required string Id { get; set; }

        public required string ArtistName { get; set; }

        public required List<DateEntryDTO> DateEntries { get; set; }
    }
}
