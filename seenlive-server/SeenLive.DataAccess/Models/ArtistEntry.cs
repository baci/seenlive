using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SeenLive.Core.Abstractions.Models;
using System.Collections.Generic;
using System.Linq;

namespace SeenLive.DataAccess.Models
{
    public class ArtistEntry : IArtistEntry
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ArtistName { get; set; }

        public IList<string> DateEntryIDs { get; private set; }

        public ArtistEntry(string id, string artistName, IEnumerable<string> dateEntryIDs)
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
