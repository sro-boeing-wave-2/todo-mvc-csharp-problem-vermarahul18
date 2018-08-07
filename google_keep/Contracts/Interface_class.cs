using google_keep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace google_keep.Contracts
{
    public class Interface_class
    {
        public interface INote
        {
            Task<IEnumerable<Note>> GetAllItems();
            Task<Note> Get(int id);
            Task Update(int id, Note note);
            Task Delete(int id);
        }
    }
}
