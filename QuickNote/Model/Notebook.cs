using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace QuickNote.Model
{
    public class Notebook : HasId
    {
        public string Id { get; set; }     
        public string UserId { get; set; }
        public string Name { get; set; }
    }
}
