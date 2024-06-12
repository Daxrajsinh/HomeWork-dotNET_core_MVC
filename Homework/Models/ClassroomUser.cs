using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Homework.CustomAttributes;
namespace Homework.Models
{
    public class ClassroomUser
    {
        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        [StringValueIn(new string[] { "Mentor", "Student" })]
        public string Role { get; set; }
        public ICollection<BlackBoard> BlackBoards { get; set; }
    }
}
