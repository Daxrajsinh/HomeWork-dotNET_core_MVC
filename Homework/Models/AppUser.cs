using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blackboard.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<Classroom> Classrooms { get; set; }
        public ICollection<ClassroomUser> ClassroomUsers { get; set; }
    }
}
