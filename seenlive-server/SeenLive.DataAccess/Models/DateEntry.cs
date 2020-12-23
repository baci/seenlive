using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SeenLive.DataAccess.Models
{
    public class DateEntry
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Date { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }
    }
}