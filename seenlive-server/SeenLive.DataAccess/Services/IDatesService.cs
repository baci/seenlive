using SeenLive.DataAccess.Models;
using System.Collections.Generic;

namespace SeenLive.DataAccess.Services
{
    public interface IDatesService
    {
        public IEnumerable<DateEntry> Get();

        public DateEntry Get(string id);

        public DateEntry Create(DateEntry newEntry);

        public bool Update(string id, DateEntry newEntry);

        public bool Remove(DateEntry oldEntry);

        public bool Remove(string id);
    }
}
