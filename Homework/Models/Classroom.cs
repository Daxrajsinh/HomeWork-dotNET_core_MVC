using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Models
{
    public class Classroom
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string title { get; set; }
        public string description { get; set; }
        public DateTime time_created { get; set; }
        public string AppUserID { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<ClassroomUser> ClassroomUsers { get; set; }
    }
}
