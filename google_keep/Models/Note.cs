using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace google_keep.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string text { get; set; }
        public List<CheckList> checklist { get; set; }
        public List<Labels> labels { get; set; }
        public bool Pinned { get; set; }
    }
}
