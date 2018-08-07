using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace google_keep.Models
{
    public class NoteContext : DbContext
    {
        public NoteContext (DbContextOptions<NoteContext> options)
            : base(options)
        {
        }

        public DbSet<google_keep.Models.Note> Note { get; set; }
        public DbSet<google_keep.Models.CheckList> CheckList { get; set; }
        public DbSet<google_keep.Models.Labels> Labels { get; set; }
    }
}
