﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Homework.ViewModels
{
    public class AssignmentViewModel
    {
        public string ClassId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFileCollection Files { get; set; }
    }
}
