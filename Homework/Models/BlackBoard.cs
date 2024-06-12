using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Models
{
    public class BlackBoard
    {
        public int Id { get; set; }
        public int ClassroomId { get; set; }
        public string AppUserId { get; set; }
        public ClassroomUser ClassroomUser { get; set; } 
        public string content { get; set; } 
        public string FilesPaths { get; set; }
    }
}
