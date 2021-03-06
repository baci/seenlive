﻿using System.Collections.Generic;

namespace SeenLive.Server.DTOs
{
    public class ArtistResponseDTO
    {
        public string Id { get; set; }

        public string ArtistName { get; set; }

        public IList<DateEntryDTO> DateEntries { get; set; }
    }
}
