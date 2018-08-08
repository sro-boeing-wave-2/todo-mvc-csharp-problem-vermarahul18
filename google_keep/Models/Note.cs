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


        public bool IsEquals(Note n)
        {

            if (Title == n.Title && text == n.text && Pinned == n.Pinned && labels.All(x => n.labels.Exists(y => y.label == x.label)) && checklist.All(x => n.checklist.Exists(y => (y.Check == x.Check && y.isChecked == x.isChecked))))
                return true;

            return false;
        }
    }
}
