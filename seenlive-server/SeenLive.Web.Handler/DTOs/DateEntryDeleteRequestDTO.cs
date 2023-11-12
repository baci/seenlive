namespace SeenLive.Web.Handler.DTOs
{
    public record DateEntryDeleteRequestDTO
    {
        public required string ArtistId { get; set; }
        public required string DateId { get; set; }
        
        public required string UserId { get; set; }
    }
}
