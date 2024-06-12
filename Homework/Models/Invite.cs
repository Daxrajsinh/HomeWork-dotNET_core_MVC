using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Models
{
    public class Invite
    {
        public int Id { get; set; }
        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; }
        public string Email { get; set; }
    }
}
