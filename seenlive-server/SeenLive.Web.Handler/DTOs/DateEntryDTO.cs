namespace SeenLive.Core.DTOs
{
    public record DateEntryDTO
    {
        public string Id { get; set; }
        public string Date { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }
    }
}
