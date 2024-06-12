using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Models
{
    public class SubmittedAssignment
    {
        public int ID { get; set; }
        public int AssignmentID { get; set; }
        public Assignment Assignment { get; set; }
        public string AppUserID { get; set; }
        public AppUser AppUser { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Files { get; set; }
    }
}
