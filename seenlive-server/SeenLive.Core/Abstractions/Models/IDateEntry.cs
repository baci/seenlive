namespace SeenLive.Core.Abstractions.Models
{
    public interface IDateEntry
    {        
        public string Id { get; set; }
        public string Date { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }
    }
}
