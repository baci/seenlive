using System.Collections.Generic;
using SeenLive.Core.Abstractions.Entities;

namespace SeenLive.DataAccess.Models;

public class UserEntity : IUserEntity
{
    public required string Id { get; set; }
    public required string Username { get; set; }
    public IList<string> ArtistEntryIDs { get; set; } = new List<string>();
    public void AddArtistEntries(IEnumerable<string> artistEntryIDs)
    {
        foreach (var artistEntryID in artistEntryIDs)
        {
            ArtistEntryIDs.Add(artistEntryID);
        }
    }
}