using System.Collections.Generic;

namespace SeenLive.Core.DTOs
{
    public record ArtistResponseDTO
    {
        public string Id { get; set; }

        public string ArtistName { get; set; }

        public IList<DateEntryDTO> DateEntries { get; set; }
    }
}
