using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Courses
{
    public class CreateUpdateCourseDto
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string LogoUrl { get; set; }
        public Guid? CollegeId { get; set; }
        public bool IsActive { get; set; } = true; // enabled/disabled
        public bool IsLifetime { get; set; } = false;
        public int? DurationInDays { get; set; } // null if lifetime
    }
}
