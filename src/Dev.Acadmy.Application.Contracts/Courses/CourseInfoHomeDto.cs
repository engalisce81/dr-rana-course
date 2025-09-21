using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Courses
{
    public class CourseInfoHomeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string LogoUrl { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public float Rating { get; set; } // average rating
        public Guid? CollegeId { get; set; }
        public string CollegeName { get; set; }
        public Guid? SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public bool AlreadyJoin { get; set; }
        public bool AlreadyRequest { get; set; }
        public int ChapterCount { get; set; }
        public int? DurationInWeeks { get; set; } // null if lifetime


    }
}
