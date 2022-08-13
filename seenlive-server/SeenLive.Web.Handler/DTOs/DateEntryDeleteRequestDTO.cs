namespace SeenLive.Core.DTOs
{
    public record DateEntryDeleteRequestDTO
    {
        public string ArtistId { get; set; }
        public string DateId { get; set; }
    }
}
