namespace SeenLive.Core.DTOs
{
    public record DateEntryDTO
    {
        public required string Id { get; set; }
        public required string Date { get; set; }
        public required string Location { get; set; }
        public required string Remarks { get; set; }
    }
}
