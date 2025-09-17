﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dev.Acadmy.Courses
{
    public class CourseDto :AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string LogoUrl { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid? CollegeId { get; set; }
        public string CollegeName { get; set; }

        // Visibility
        public bool IsActive { get; set; } = true; // enabled/disabled

        // Purchase & duration
        public bool IsLifetime { get; set; } = false;
        public int? DurationInDays { get; set; } // null if lifetime
    }
}
