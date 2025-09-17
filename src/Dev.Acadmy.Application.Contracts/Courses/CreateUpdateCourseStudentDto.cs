using System;

namespace Dev.Acadmy.Courses
{
    public class CreateUpdateCourseStudentDto
    {
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public bool IsSubscibe { get; set; }
    }
}
