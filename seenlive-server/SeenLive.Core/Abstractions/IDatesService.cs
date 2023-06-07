using System.Collections.Generic;
using SeenLive.Core.Abstractions.Models;

namespace SeenLive.Core.Abstractions
{
    public interface IDatesService
    {
        public IEnumerable<IDateEntry> Get();

        public IDateEntry Get(string id);

        public bool Update(string id, IDateEntry newEntry);

        public bool Remove(IDateEntry oldEntry);

        public bool Remove(string id);

        IDateEntry Create(string date, string? location, string? remarks);
    }
}
