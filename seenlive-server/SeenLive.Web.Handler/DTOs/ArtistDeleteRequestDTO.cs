namespace SeenLive.Web.Handler.DTOs
{
    public record ArtistDeleteRequestDTO
    {
        public required string UserId { get; set; }
        
        public required string ArtistEntryId { get; set; }
    }
}
