using System.Collections.Generic;

namespace SeenLive.Core.DTOs
{
    public record ArtistResponseDTO
    {
        public required string Id { get; set; }

        public required string ArtistName { get; set; }

        public required IList<DateEntryDTO> DateEntries { get; set; }
    }
}
