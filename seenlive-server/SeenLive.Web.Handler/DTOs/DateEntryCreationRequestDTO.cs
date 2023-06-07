namespace SeenLive.Web.Handler.DTOs
{
    public record DateEntryCreationRequestDTO
    {
        public required string Date { get; set; }
        public string? Location { get; set; }
        public string? Remarks { get; set; }
    }
}
