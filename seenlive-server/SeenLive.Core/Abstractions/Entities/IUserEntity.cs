using System.Collections.Generic;

namespace SeenLive.Core.Abstractions.Entities;

public interface IUserEntity
{
    public string Id { get; set; }
    
    public string Username { get; set; }
    
    public IList<string> ArtistEntryIDs { get; set; }
    
    void AddArtistEntries(IEnumerable<string> artistEntryIDs);
}