using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Homework.ViewModels
{
    public class BlackBoardViewModel
    {
        public string ClassId { get; set; }
        public string content { get; set; }
        public IFormFileCollection Files { get; set; }
    }
}
