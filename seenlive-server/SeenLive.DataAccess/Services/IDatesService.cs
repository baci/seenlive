using SeenLive.DataAccess.Models;
using System.Collections.Generic;

namespace SeenLive.DataAccess.Services
{
    public interface IDatesService
    {
        public IEnumerable<DateEntry> Get();

        public DateEntry Get(string id);

        public DateEntry Create(DateEntry newEntry);

        public void Update(string id, DateEntry newEntry);

        public void Remove(DateEntry oldEntry);

        public void Remove(string id);
    }
}
