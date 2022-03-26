using System.Collections.Generic;
using SeenLive.Core.DTOs;

namespace SeenLive.Web.Handler.DTOs
{
    public record ArtistCreationRequestDTO
    {
        public string? ArtistName { get; set; }

        public IEnumerable<DateEntryCreationRequestDTO> DateEntryRequests { get; set; } =
            new List<DateEntryCreationRequestDTO>();
    }
}
