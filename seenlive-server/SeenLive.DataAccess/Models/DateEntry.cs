using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SeenLive.Core.Abstractions.Models;

namespace SeenLive.DataAccess.Models
{
    public class DateEntry : IDateEntry
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }
        public required string Date { get; set; }
        public string? Location { get; set; }
        public string? Remarks { get; set; }
    }
}