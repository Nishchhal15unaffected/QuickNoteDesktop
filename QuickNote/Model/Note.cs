using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace QuickNote.Model
{
    public interface HasId
    {
        public string Id { get; set; }
    }
    public class Note : HasId
    {   
        public string Id { get; set; }
        public string Notebook { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string FileLocation { get; set; } 
    }
}
