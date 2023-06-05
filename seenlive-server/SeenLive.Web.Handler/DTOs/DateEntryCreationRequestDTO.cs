namespace SeenLive.Core.DTOs
{
    public record DateEntryCreationRequestDTO
    {
        public required string Date { get; set; }
        public string? Location { get; set; }
        public string? Remarks { get; set; }
    }
}
