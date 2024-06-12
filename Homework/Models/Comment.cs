using Homework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int BlackBoardId { get; set; }
        public BlackBoard BlackBoard { get; set; }
        public string Content { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}