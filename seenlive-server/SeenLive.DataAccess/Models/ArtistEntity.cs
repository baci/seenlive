using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using SeenLive.Core.Abstractions.Entities;

namespace SeenLive.DataAccess.Models
{
    public class ArtistEntity : IArtistEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ArtistName { get; set; }

        public IList<string> DateEntryIDs { get; set; }

        public ArtistEntity(string id, string artistName, IEnumerable<string> dateEntryIDs)
        {            
            Id = id;
            ArtistName = artistName;
            DateEntryIDs = dateEntryIDs.ToList();
        }

        public void AddDateEntries(IEnumerable<string> dateEntryIDs)
        {
            DateEntryIDs = DateEntryIDs.Concat(dateEntryIDs).ToList();
        }
    }
}
