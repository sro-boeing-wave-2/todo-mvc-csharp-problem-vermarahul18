using google_keep.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace test_class1
{
        public interface INote
        {
            Task<IEnumerable<Note>> GetAllItems();
            Task<Note> Get(int id);
            Task Update(int id, Note note);
            Task Delete(int id);
        }


    public class Interface_class : INote
    {
        private List<Note> Notes {get; set;}

     
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Note> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Note>> GetAllItems()
        {

            throw new NotImplementedException();
        }

        public Task Update(int id, Note note)
        {
            throw new NotImplementedException();
        }
    }
}
