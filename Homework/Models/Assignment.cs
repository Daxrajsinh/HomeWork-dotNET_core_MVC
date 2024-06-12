using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Homework.Models
{
    public class Assignment
    {
        public int ID { get; set; }
        public int ClassroomID { get; set; }
        public Classroom Classroom { get; set; }
        public string AppUserID { get; set; }
        public AppUser AppUser { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Files { get; set; }
    }
}
