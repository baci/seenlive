using SeenLive.Core.Abstractions.Models;
using System.Collections.Generic;

namespace SeenLive.Core.Services
{
    public interface IDatesService
    {
        public IEnumerable<IDateEntry> Get();

        public IDateEntry Get(string id);

        public IDateEntry Create(IDateEntry newEntry);

        public bool Update(string id, IDateEntry newEntry);

        public bool Remove(IDateEntry oldEntry);

        public bool Remove(string id);
    }
}
